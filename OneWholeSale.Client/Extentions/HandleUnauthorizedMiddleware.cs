using Microsoft.AspNetCore.Authentication;
using System.Net;

namespace OneWholeSale.Client.Extentions
{
    public class HandleUnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public HandleUnauthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await context.SignOutAsync();
                // redirect to the login page
                context.Response.Redirect("/Account/Login");
            }
        }
    }

}
