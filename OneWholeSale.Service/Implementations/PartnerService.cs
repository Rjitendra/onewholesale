using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneWholeSale.Model.Context;
using OneWholeSale.Model.Dto;
using OneWholeSale.Model.Dto.Partner;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Model.Entity.Partner;
using OneWholeSale.Model.Entity.SalesPerson;
using OneWholeSale.Model.Enums;
using OneWholeSale.Service.Interfaces;
using OneWholeSale.Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Service.Implementations
{
    public class PartnerService : Ipartner
    {
        private ApiContext Db { get; set; }


    private IMapper _mapper;
    private CurrentUser CurrentUser { get; set; }
    private IUserService UserService { get; }
    public PartnerService(
         ApiContext db,
         IHttpContextAccessor context,
         IUserService userService,
         IMapper mapper
       )
    {
        this.Db = db;
        _mapper = mapper;
        UserService = userService;

        this.CurrentUser = new CurrentUser(context.HttpContext.User);
    }
    public async Task<Result<bool>> AddPartner(PartnerDto dto)
        {
            using (var transaction = this.Db.Database.BeginTransaction())
            {
                try
                {
                    dto.Addon = DateTime.Now;
                    dto.PartnerCode = PartnerCode();
                    // get all roles
                    var roleList = await this.UserService.Roles();

                    // Get the role ID for "Tenant" role
                    var roleId = roleList.Entity
                                   .Where(x => x.Name == ApplicationUserRole.Partner.ToString())
                                   .Select(x => x.Id)
                                   .FirstOrDefault();



                    // Create a list of ApplicationUserDto objects
                    var users = new ApplicationUserDto()
                    {
                        FirstName = dto.Name,
                        LastName = "",
                        UserName = dto.Email,
                        Email = dto.Email,
                        Password = dto.Email + "-Sale64#",
                        IsEnabled = true,
                        UserRoles = new List<ApplicationUserRoleDto>()
                                           {
                                                new ApplicationUserRoleDto(){ Id = roleId, Name = ApplicationUserRole.Partner.ToString() }
                                           }
                    };

                    // Create the users using UserService.CreateUser method
                    var userResponse = this.UserService.CreateUser(users);

                    if (userResponse <= 0)
                    {
                        return Result<bool>.Failure("Failed to create Sales Person");
                    }

                    var applicationUser = await this.Db.Users
                                                   .Where(a => a.UserName == users.UserName || a.Email == users.Email).SingleOrDefaultAsync();


                    var result = _mapper.Map<Partner>(dto);
                    result.UserId = applicationUser.Id;

                    // Add the SalesPerson to the DbContext's SalesPerson DbSet.
                    await this.Db.Partner.AddAsync(result);

                    // Save the changes to the database.
                    await this.Db.SaveChangesAsync();

                    transaction.Commit();
                    // Return a Result object indicating success and a value of true.
                    return Result<bool>.Success(true);
                }

                catch (Exception ex)
                {

                    transaction.Rollback();

                    var applicationUser = await this.Db.Users.Where(a => a.UserName == dto.Email || a.Email == dto.Email).SingleOrDefaultAsync();
                    if (applicationUser != null)
                    {
                        bool res = UserService.DeleteApplicationUser(applicationUser.Id);
                        return Result<bool>.Failure("Failed to create Partner");
                    }
                    return Result<bool>.Failure("Failed to create Partner");
                }
            }
        }
        public string PartnerCode()
        {

            var data = Db.Partner.ToList().Count;


            string data2 = "";
            if (data == 0)
            {
                int id = 1;


                data2 = "Partner-" + id;
            }
            else
            {
                int id = data + 1;
                data2 = "Partner-" + id;
            }
            return data2;
        }
        public async Task<Result<bool>> DeletePartner(int id)
        {
            try
            {
                if (id == 0)
                {
                    return Result<bool>.NotFound();
                }

                // need to implement
                //  if sales person associate with other group,then we can not delete untill we discontinue sales person in other group
                var Partner = await this.Db.Partner.Where(x => x.Id == id).SingleOrDefaultAsync();
                var applicationUser = await this.Db.Users.Where(a => a.Id == id).SingleOrDefaultAsync();
                if (applicationUser != null)
                {
                    bool res = UserService.DeleteApplicationUser(applicationUser.Id);
                }
                this.Db.Partner.Remove(Partner);
                await this.Db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error in Deleting Partner");
            }
        }

        public async Task<Result<PartnerDto>> GetPartner(int id)
        {
            try
            {
                var Partner = await this.Db.Partner.Where(x => x.Id == id).SingleOrDefaultAsync();

                var result = _mapper.Map<PartnerDto>(Partner);
                return Result<PartnerDto>.Success(result);
            }
            catch (Exception ex) { return Result<PartnerDto>.Failure("Error in getting Sales Person"); }
        }

        public async Task<Result<bool>> UpdatePartner(PartnerDto dto)
        {
            try
            {

                var Partner = _mapper.Map<Partner>(dto);

                this.Db.Partner.Attach(Partner);
                this.Db.Entry(Partner).State = EntityState.Modified;

                await this.Db.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error in Updating Partner");
            }
        }
    }
}
