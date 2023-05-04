namespace OneWholeSale.API.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using OneWholeSale.API.Config;
    using OneWholeSale.Model.Dto.Login;
    using OneWholeSale.Model.Entity;
    using OneWholeSale.Model.Enums;
    using OneWholeSale.Model.Utility;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;



    // Constants.cs
    public static class Constants
    {

        public static string[] RoleNames = {
            ApplicationUserRole.None.ToString(),
            ApplicationUserRole.Admin.ToString(),
            ApplicationUserRole.FulfillmentCenter.ToString(),
            ApplicationUserRole.Partner.ToString(),
            ApplicationUserRole.Kirana.ToString(),
            ApplicationUserRole.SalesPerson.ToString()
        };
    }


    [Route("api/[controller]")]
    [ApiController]

    public class LogInController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationUserIdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IdentityServerSettings _serverSettings;
        public LogInController(
           UserManager<ApplicationUser> userManager,
           RoleManager<ApplicationUserIdentityRole> roleManager,
           IdentityServerSettings serverSettings,
           IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _serverSettings = serverSettings;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] loginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roleNames = await _userManager.GetRolesAsync(user);
                var applicationRoles = ApplicationUserRoleExtensions.GetApplicationUserRoles().ToList();
                var claims = await _userManager.GetClaimsAsync(user);
                // Join User's roles with the application's roles.  We do this to get the ID of the role
                var userAppRoles = roleNames.Join(applicationRoles,
                    roleName => roleName,
                    appRole => appRole.Name,
                    (roleName, appRole) => new { appRole });
                // Create claims for the RoleId
                var roleIdClaims = userAppRoles.Select(r => new Claim(ApplicationClaims.RoleId, ((int)r.appRole.Id).ToString()));
                // Create claims for the RoleName
                var roleNameClaims = userAppRoles.Select(r => new Claim(ApplicationClaims.RoleName, r.appRole.Name));

                var authClaims = new List<Claim>()
                {
                      
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                // Add the roleIdClaims and roleNameClaims to the authClaims list
                authClaims.AddRange(claims);
                authClaims.AddRange(roleIdClaims);
                authClaims.AddRange(roleNameClaims);




                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_serverSettings.ApiSecret));

            var token = new JwtSecurityToken(
                issuer: _serverSettings.ValidIssuer,
                audience: _serverSettings.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
