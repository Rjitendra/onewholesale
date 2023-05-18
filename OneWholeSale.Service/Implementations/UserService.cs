namespace OneWholeSale.Service.Implementations
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using OneWholeSale.Model.Context;
    using OneWholeSale.Model.Dto;
    using OneWholeSale.Model.Entity;
    using OneWholeSale.Model.Enums;
    using OneWholeSale.Service.Interfaces;
    using OneWholeSale.Service.Utility;
    using System.Linq;

    public class UserService : IUserService
    {
        public UserService(ApiContext db, IHttpContextAccessor context)

        {
            this.Db = db;
            this.CurrentUser = new CurrentUser(context.HttpContext.User);
        }

        /// <summary>
        /// Current user.
        /// </summary>
        private CurrentUser CurrentUser { get; set; }

        private ApiContext Db { get; set; }

        public int GetUserId()
        {
            return CurrentUser.ApplicationUserId;
        }
        public int CreateUser(ApplicationUserDto entity)
        {
            try
            {
                var userExisted = this.Db.Users.Where(a => a.UserName == entity.UserName || a.Email == entity.Email).SingleOrDefault();

                if (userExisted != null)
                {
                    // if Username is duplicated returns -1 and its handled in Angualr side to display error message
                    return -1;
                }

                var appUser = new ApplicationUser(entity);
                this.Db.Users.Add(appUser);
                this.Db.SaveChanges();

                if (appUser.Id > 0)
                {
                    this.AddUserToTheRole(entity, appUser.Id);

                    // Adding user information into the claims
                    this.AddOrUpdateUserClaim(IdentityClaims.FullName, $"{entity.FirstName} {entity.LastName}", appUser.Id);
                    this.AddOrUpdateUserClaim(IdentityClaims.FirstName, entity.FirstName, appUser.Id);
                    this.AddOrUpdateUserClaim(IdentityClaims.LastName, entity.LastName, appUser.Id);
                    this.AddOrUpdateUserClaim(IdentityClaims.Email, entity.Email, appUser.Id);

                    return appUser.Id;
                }
                else
                {
                    // if user info is not saved and its handled in Angualr side to display error message
                    return 0;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public int CreateMultipleUser(IList<ApplicationUserDto> dto)
        {
            try
            {
                var userExisted = this.Db.Users.Where(a => (dto.Select(x => x.Email).ToList()).Contains(a.Email)).AsNoTracking().ToList();
                if (userExisted.Count() != 0) { return -1; }
                int id = 0;
                foreach (var entity in dto)
                {
                    var result = CreateUser(entity);

                    id = result;
                }

                return id;
            }
            catch (Exception ex) { return -1; }
        }

        public int UpdateUser(ApplicationUserDto entity)
        {
            var user = this.Db.Users.Find(entity.ApplicationUserId);
            if (user == null)
            {
                return 0;
            }
            // base requirement need  to modify
            user.Update(entity);
            this.Db.SaveChanges();

            return user.Id;
        }

        public bool DeleteApplicationUsers(List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var user = this.Db.Users.Where(x => x.Id == id).SingleOrDefault();
                    this.Db.Users.Remove(user);
                    this.Db.SaveChanges();
                };

                return true;
            }
            catch (Exception ex)
            {
                // log the exception here
                return false;
            }
        }

        public bool DeleteApplicationUser(int id)
        {
            try
            {
                var user = this.Db.Users.Where(x => x.Id == id).SingleOrDefault();
                this.Db.Users.Remove(user);
                this.Db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // log the exception here
                return false;
            }
        }

        public async Task<Result<IEnumerable<ApplicationUserRoleDto>>> Roles()
        {
            try
            {
                var UserRoles = await this.Db.Roles.Select(a => new ApplicationUserRoleDto { Id = (ApplicationUserRole)a.Id, Name = a.Name }).ToListAsync();
                return Result<IEnumerable<ApplicationUserRoleDto>>.Success(UserRoles);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Method for Adding user into the role
        /// </summary> 
        /// <param name="entity"></param>
        /// <param name="userId"></param>
        private void AddUserToTheRole(ApplicationUserDto entity, int userId)
        {
            try
            {
                var userRole = new List<IdentityUserRole<int>>();

                // Create role(s) for user
                foreach (var role in entity.UserRoles)
                {
                    userRole.Add(new IdentityUserRole<int>()
                    {
                        RoleId = (int)role.Id,
                        UserId = userId
                    });
                }

                this.Db.UserRoles.AddRange(userRole);
                this.Db.SaveChanges();
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Adds or updates the specified user claim type for the specified user.
        /// </summary>
        /// <param name="claimType">Claim type.</param>
        /// <param name="claimValue">Claim value.</param>
        /// <param name="applicationUserId">User whose claims are to be modified.</param>
        /// <returns></returns>

        private void AddOrUpdateUserClaim(string claimType, string claimValue, int applicationUserId)
        {
            try
            {
                // fetch existing claim
                var claim = this.Db.UserClaims.SingleOrDefault(x => x.ClaimType == claimType && x.UserId == applicationUserId);

                // if no claim exists, create one
                if (claim == null)
                {
                    this.Db.UserClaims.Add(new IdentityUserClaim<int> { ClaimType = claimType, ClaimValue = claimValue, UserId = applicationUserId });
                    this.Db.SaveChanges();
                }
                else
                {
                    // otherwise, update the claim
                    claim.ClaimValue = claimValue;
                }
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
