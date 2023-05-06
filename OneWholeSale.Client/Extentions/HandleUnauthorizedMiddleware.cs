using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized && context.User.Identity.IsAuthenticated)
            {
                // Clear the existing authentication cookie
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                // Redirect to the login page
                context.Response.Redirect("/Account/Login");
            }
        }
    }


}
