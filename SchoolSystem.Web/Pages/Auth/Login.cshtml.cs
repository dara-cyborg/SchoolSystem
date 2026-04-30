using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Web.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly WebAuthService _authService;

    public LoginModel(WebAuthService authService)
    {
        _authService = authService;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public string ReturnUrl { get; set; } = string.Empty;

    public class InputModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToPage("/Index");
        }

        ReturnUrl = returnUrl ?? Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (ModelState.IsValid)
        {
            var (principal, apiToken) = await _authService.LoginAndGetTokenAsync(Input.Username, Input.Password);

            if (principal != null)
            {
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    principal, 
                    authProperties);

                // Store JWT token in cookie for API calls (persists across restarts)
                var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                };
                Response.Cookies.Append("ApiToken", apiToken ?? "", cookieOptions);

                return LocalRedirect(ReturnUrl);
            }
            
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}
