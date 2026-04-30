using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolSystem.Web.Services;

namespace SchoolSystem.Web.Pages;

public abstract class AuthenticatedPageModel : PageModel
{
    protected readonly ApiHttpClientFactory ApiClientFactory;

    protected AuthenticatedPageModel(ApiHttpClientFactory apiClientFactory)
    {
        ApiClientFactory = apiClientFactory;
    }

    protected IActionResult? CheckToken()
    {
        if (!ApiClientFactory.HasToken)
        {
            return RedirectToPage("/Auth/Login");
        }
        return null;
    }
}
