using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace OneWholeSale.Client.Extentions
{
    public class HandleApiExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HandleApiExceptionFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (context.Exception is HttpRequestException httpException && httpException.StatusCode == HttpStatusCode.Unauthorized)
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Result = new RedirectToActionResult("Login", "Account", null);
                context.ExceptionHandled = true;
            }
        }
    }
}
