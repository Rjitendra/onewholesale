namespace OneWholeSale.Client.Extentions
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using System.Net;
 
    public class UnauthorizedRequestHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnauthorizedRequestHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (response.Headers.WwwAuthenticate.Any())
                {
                    var wwwAuthenticateHeader = response.Headers.WwwAuthenticate.First();
                    if (wwwAuthenticateHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                    {
                        var error = wwwAuthenticateHeader.Parameter.Split(',').Select(p => p.Trim().Split('=')).ToDictionary(kv => kv[0], kv => kv[1].Trim('"'));
                        if (error.ContainsKey("error") && error.ContainsKey("error_description"))
                        {
                            var errorMessage = error["error"] + ": " + error["error_description"];
                            // Handle the bearer error
                            // Sign the user out
                            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            // Redirect the user to the login page
                            _httpContextAccessor.HttpContext.Response.Redirect("/Account/Login");
                        }
                    }
                }
            }

            return response;
        }
    }
}
