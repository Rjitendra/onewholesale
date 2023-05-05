using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authentication;

namespace OneWholeSale.Client.Extentions
{
    public class HandleApiExceptionFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is HttpRequestException httpException && httpException.StatusCode == HttpStatusCode.Unauthorized)
            {
                await context.HttpContext.SignOutAsync();
                context.Result = new RedirectToActionResult("Login", "Account", null);
                context.ExceptionHandled = true;
            }
        }
    }
}
