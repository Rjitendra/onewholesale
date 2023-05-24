using AutoMapper;
using Microsoft.AspNetCore.Http;
using OneWholeSale.Model.Context;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Model.Dto;
using OneWholeSale.Model.Entity.SalesPerson;
using OneWholeSale.Model.Enums;
using OneWholeSale.Service.Interfaces;
using OneWholeSale.Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneWholeSale.Model.Dto.FC;
using Microsoft.EntityFrameworkCore;
using OneWholeSale.Model.Entity.FullFillCenter;

namespace OneWholeSale.Service.Implementations
{
    public class FC: IFC
    {
        private ApiContext Db { get; set; }


        private IMapper _mapper;
        private CurrentUser CurrentUser { get; set; }
        private IUserService UserService { get; }
        public FC(
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

        public async Task<Result<bool>> AddNewFC(FullFillmentCenterDto dto)
        {
            using (var transaction = this.Db.Database.BeginTransaction())
            {
                try
                {
                    // get all roles
                    var roleList = await this.UserService.Roles();

                    // Get the role ID for "Tenant" role
                    var roleId = roleList.Entity
                                   .Where(x => x.Name == ApplicationUserRole.FulfillmentCenter.ToString())
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
                                                new ApplicationUserRoleDto(){ Id = roleId, Name = ApplicationUserRole.FulfillmentCenter.ToString() }
                                           }
                    };

                    // Create the users using UserService.CreateUser method
                    var userResponse = this.UserService.CreateUser(users);

                    if (userResponse <= 0)
                    {
                        return Result<bool>.Failure("Failed to create Fulfillment Center");
                    }

                    var applicationUser = await this.Db.Users
                                                   .Where(a => a.UserName == users.UserName || a.Email == users.Email).SingleOrDefaultAsync();


                    var result = _mapper.Map<FullFillmentCenter>(dto);
                    result.UserId = applicationUser.Id;

                    // Add the SalesPerson to the DbContext's SalesPerson DbSet.
                    await this.Db.FullFillmentCenter.AddAsync(result);

                    // Save the changes to the database.
                    await this.Db.SaveChangesAsync();

                    foreach (var districtId in dto.MapDistrictToFcc.Select(d => d.DistrictId))
                    {
                        var mapDistrictToFc = new MapDistrictToFc
                        {
                            FcId = dto.Id,
                            DistrictId = districtId
                        };

                        await this.Db.MapDistrictToFc.AddAsync(mapDistrictToFc);
                    }

                    // Add mapping of salespersons to the fulfillment center
                    foreach (var salesPersonId in dto.MapSalesPersonToFCc.Select(s => s.SalesPersonId))
                    {
                        var mapSalesPersonToFC = new MapSalesPersonToFC
                        {
                            FcId = dto.Id,
                            SalesPersonId = salesPersonId
                        };

                        await this.Db.MapSalesPersonToFC.AddAsync(mapSalesPersonToFC);
                    }

                    transaction.Commit();
                    // Return a Result object indicating success and a value of true.
                    return Result<bool>.Success(true);
                }

                catch (Exception ex)
                {

                    transaction.Rollback();

                    var applicationUser = await this.Db.Users
                                                    .Where(a => a.UserName == dto.Email || a.Email == dto.Email).SingleOrDefaultAsync();
                    if (applicationUser != null)
                    {
                        bool res = UserService.DeleteApplicationUser(applicationUser.Id);

                        return Result<bool>.Failure("Failed to create Sales Person");
                    }
                    return Result<bool>.Failure("Failed to create Sales Person");
                }
            }

            


        }

        public async Task<Result<bool>> DeleteFc(int id)
        {
            try
            {
                if (id == 0)
                {
                    return Result<bool>.NotFound();
                }

                // need to implement
                //  if sales person associate with other group,then we can not delete untill we discontinue sales person in other group
                var FullFillmentCenter = await this.Db.FullFillmentCenter.Where(x => x.Id == id).SingleOrDefaultAsync();
                var applicationUser = await this.Db.Users.Where(a => a.Id == id).SingleOrDefaultAsync();
                if (applicationUser != null)
                {
                    bool res = UserService.DeleteApplicationUser(applicationUser.Id);
                }
                this.Db.FullFillmentCenter.Remove(FullFillmentCenter);
                await this.Db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error in Deleting Fulfillment Center);
                }
        }

    }
}
