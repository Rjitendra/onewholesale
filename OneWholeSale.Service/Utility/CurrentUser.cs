namespace OneWholeSale.Service.Utility
{
    using OneWholeSale.Model.Enums;
    using OneWholeSale.Model.Utility;
    using System.Security.Claims;

    public class CurrentUser
    {
        public CurrentUser(ClaimsPrincipal user)
        {
            // set application user id
            this.ApplicationUserId = Convert.ToInt32(user.FindFirstValue(IdentityClaims.Subject));

            // set user role
            var role = user.FindAll(ApplicationClaims.RoleId);
            if (role != null)
            {
                this.Role = new List<ApplicationUserRole>();
                foreach (Claim rol in role)
                {
                    this.Role.Add((ApplicationUserRole)Enum.Parse(typeof(ApplicationUserRole), rol.Value));
                }
            }
        }

        /// <summary>
        /// User subject claim (e.g. ApplicationUser PK). Relates to <see cref="Entity.ApplicationUser"/>.
        /// </summary>
        public int ApplicationUserId { get; }

        /// <summary>
        /// User's role.
        /// </summary>
        public List<ApplicationUserRole> Role { get; }
    }
}