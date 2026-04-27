using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SchoolSystem.Desktop.Services {
    public sealed class ApiClient {
        private static readonly Lazy<ApiClient> _instance = new(() => new ApiClient());
        private static readonly HttpClient _httpClient = new() {
            BaseAddress = new Uri("http://localhost:5041")
        };

        private static readonly JsonSerializerOptions _jsonOptions = new() {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        private ApiClient() {
            Roles = new List<string>();
        }

        public static ApiClient Instance => _instance.Value;
        public string? Token { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);
        public List<string> Roles { get; private set; }
        public string? UserName { get; private set; }

        public async Task<string> LoginAsync(string name, string password) {
            var requestBody = new {
                name,
                password
            };

            using var response = await _httpClient.PostAsync("/api/auth/login", BuildJsonContent(requestBody));

            if (!response.IsSuccessStatusCode) {
                throw new InvalidOperationException("Invalid credentials.");
            }

            var token = await ExtractTokenAsync(response);
            if (string.IsNullOrWhiteSpace(token)) {
                throw new InvalidOperationException("Invalid credentials.");
            }

            Token = token;
            ApplyAuthorizationHeader();
            ParseJwtClaims(token);

            return token;
        }

        public void Logout() {
            Token = null;
            Roles = new List<string>();
            UserName = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<T?> GetAsync<T>(string endpoint) {
            ApplyAuthorizationHeader();

            using var response = await _httpClient.GetAsync(endpoint);
            if (response.StatusCode == HttpStatusCode.NotFound) {
                return default;
            }

            if (!response.IsSuccessStatusCode) {
                throw await CreateRequestExceptionAsync(response);
            }

            return await DeserializeResponseAsync<T>(response);
        }

        public async Task<T?> PostAsync<T>(string endpoint, object body) {
            ApplyAuthorizationHeader();

            using var response = await _httpClient.PostAsync(endpoint, BuildJsonContent(body));
            if (!response.IsSuccessStatusCode) {
                throw await CreateRequestExceptionAsync(response);
            }

            return await DeserializeResponseAsync<T>(response);
        }

        public async Task<T?> PutAsync<T>(string endpoint, object body) {
            ApplyAuthorizationHeader();

            using var response = await _httpClient.PutAsync(endpoint, BuildJsonContent(body));
            if (!response.IsSuccessStatusCode) {
                throw await CreateRequestExceptionAsync(response);
            }

            return await DeserializeResponseAsync<T>(response);
        }

        public async Task<bool> DeleteAsync(string endpoint) {
            ApplyAuthorizationHeader();

            using var response = await _httpClient.DeleteAsync(endpoint);
            if (response.StatusCode == HttpStatusCode.NoContent) {
                return true;
            }

            if (response.StatusCode == HttpStatusCode.NotFound) {
                return false;
            }

            throw await CreateRequestExceptionAsync(response);
        }

        private void ApplyAuthorizationHeader() {
            if (string.IsNullOrWhiteSpace(Token)) {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        }

        private static StringContent BuildJsonContent(object body) {
            var json = JsonSerializer.Serialize(body, _jsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response) {
            if (response.Content is null) {
                return default;
            }

            var payload = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(payload)) {
                return default;
            }

            return JsonSerializer.Deserialize<T>(payload, _jsonOptions);
        }

        private static async Task<string> ExtractTokenAsync(HttpResponseMessage response) {
            var payload = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(payload)) {
                return string.Empty;
            }

            try {
                using var document = JsonDocument.Parse(payload);

                if (document.RootElement.ValueKind == JsonValueKind.Object &&
                    document.RootElement.TryGetProperty("token", out var tokenProperty) &&
                    tokenProperty.ValueKind == JsonValueKind.String) {
                    return tokenProperty.GetString() ?? string.Empty;
                }

                if (document.RootElement.ValueKind == JsonValueKind.String) {
                    return document.RootElement.GetString() ?? string.Empty;
                }
            }
            catch (JsonException) {
                // Ignore parse errors and treat payload as raw token content.
            }

            return payload.Trim('"', ' ', '\r', '\n', '\t');
        }

        private void ParseJwtClaims(string token) {
            Roles = new List<string>();
            UserName = null;

            var parts = token.Split('.');
            if (parts.Length < 2) {
                return;
            }

            var payloadSegment = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            var paddedPayload = payloadSegment.PadRight(payloadSegment.Length + ((4 - payloadSegment.Length % 4) % 4), '=');

            try {
                var payloadBytes = Convert.FromBase64String(paddedPayload);
                var payloadJson = Encoding.UTF8.GetString(payloadBytes);

                using var document = JsonDocument.Parse(payloadJson);
                var root = document.RootElement;

                UserName = ReadFirstStringClaim(
                    root,
                    "name",
                    "unique_name",
                    "preferred_username",
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
                );

                var roleClaimNames = new[] {
                    "role",
                    "roles",
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                };

                var roleSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var claimName in roleClaimNames) {
                    foreach (var role in ReadStringOrArrayClaim(root, claimName)) {
                        roleSet.Add(role);
                    }
                }

                Roles = roleSet.ToList();
            }
            catch (FormatException) {
                Roles = new List<string>();
                UserName = null;
            }
            catch (JsonException) {
                Roles = new List<string>();
                UserName = null;
            }
        }

        private static string? ReadFirstStringClaim(JsonElement root, params string[] claimNames) {
            foreach (var claimName in claimNames) {
                if (!root.TryGetProperty(claimName, out var property) || property.ValueKind != JsonValueKind.String) {
                    continue;
                }

                var value = property.GetString();
                if (!string.IsNullOrWhiteSpace(value)) {
                    return value;
                }
            }

            return null;
        }

        private static IEnumerable<string> ReadStringOrArrayClaim(JsonElement root, string claimName) {
            if (!root.TryGetProperty(claimName, out var claimValue)) {
                yield break;
            }

            if (claimValue.ValueKind == JsonValueKind.String) {
                var singleValue = claimValue.GetString();
                if (!string.IsNullOrWhiteSpace(singleValue)) {
                    yield return singleValue;
                }

                yield break;
            }

            if (claimValue.ValueKind != JsonValueKind.Array) {
                yield break;
            }

            foreach (var item in claimValue.EnumerateArray()) {
                if (item.ValueKind != JsonValueKind.String) {
                    continue;
                }

                var value = item.GetString();
                if (!string.IsNullOrWhiteSpace(value)) {
                    yield return value;
                }
            }
        }

        private static async Task<InvalidOperationException> CreateRequestExceptionAsync(HttpResponseMessage response) {
            var defaultMessage = ((int)response.StatusCode).ToString();
            var payload = response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(payload)) {
                return new InvalidOperationException(defaultMessage);
            }

            try {
                using var document = JsonDocument.Parse(payload);
                if (document.RootElement.ValueKind == JsonValueKind.Object &&
                    document.RootElement.TryGetProperty("message", out var messageProperty) &&
                    messageProperty.ValueKind == JsonValueKind.String) {
                    var message = messageProperty.GetString();
                    if (!string.IsNullOrWhiteSpace(message)) {
                        return new InvalidOperationException(message);
                    }
                }
            }
            catch (JsonException) {
                // Fall back to status code when payload is not a JSON object.
            }

            return new InvalidOperationException(defaultMessage);
        }
    }
}
