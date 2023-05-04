namespace OneWholeSale.Model.Entity
{
    using Microsoft.AspNetCore.Identity;
    using OneWholeSale.Model.Dto;
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser(string email)
                    : base(email)
        {
            this.Email = email;
        }

        public ApplicationUser(ApplicationUserDto entity)
        {
            this.Id = entity.ApplicationUserId;
            this.AssignEntityValue(entity);
        }

        private ApplicationUser()
        {
        }

        // Property to set user is enabled or not
        public bool IsEnabled { get; set; }

        public void Update(ApplicationUserDto entity)
        {
            this.Id = entity.ApplicationUserId;
            this.AssignEntityValue(entity, true);
        }

        public void AssignEntityValue(ApplicationUserDto entity, bool isUpdate = false)
        {
            this.UserName = entity.UserName;
            this.Email = entity.Email;
            this.IsEnabled = entity.IsEnabled;
            this.NormalizedUserName = entity.UserName.ToUpper();
            this.NormalizedEmail = entity.Email.ToUpper();
            this.EmailConfirmed = false;

            if (!string.IsNullOrEmpty(entity.Password))
            {
                var options = new PasswordHasherOptions();
                options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
                var hasher = new PasswordHasher<ApplicationUser>();
                this.PasswordHash = hasher.HashPassword(this, entity.Password);
                if (!isUpdate)
                {
                    this.SecurityStamp = Guid.NewGuid().ToString();
                }
            }
        }
    }
}