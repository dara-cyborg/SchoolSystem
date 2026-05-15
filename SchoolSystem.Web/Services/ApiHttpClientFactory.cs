using System.Net.Http.Headers;

namespace SchoolSystem.Web.Services;

public class ApiHttpClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public ApiHttpClientFactory(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public bool HasToken =>
        !string.IsNullOrEmpty(
            _httpContextAccessor.HttpContext?.Request.Cookies["ApiToken"]);

    public HttpClient CreateAuthenticatedClient()
    {
        var client = _httpClientFactory.CreateClient();
        var token = _httpContextAccessor.HttpContext?
            .Request.Cookies["ApiToken"];
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        return client;
    }

    public string GetApiBaseUrl() =>
        _configuration["ApiBaseUrl"] ?? "http://localhost:5041";
}
