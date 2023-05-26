using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OneWholeSale.Model.Context;
using OneWholeSale.Model.Dto;
using OneWholeSale.Model.Dto.Partner;
using OneWholeSale.Model.Entity.FullFillCenter;
using OneWholeSale.Model.Entity.Kirana;
using OneWholeSale.Model.Entity.Partner;
using OneWholeSale.Model.Enums;
using OneWholeSale.Service.Implementations.Interfaces;
using OneWholeSale.Service.Interfaces;
using OneWholeSale.Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Service.Implementations
{
    public class KiranaService : IKirana
    {
        private ApiContext Db { get; set; }


        private IMapper _mapper;
        private CurrentUser CurrentUser { get; set; }
        private IUserService UserService { get; }
        public KiranaService(
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

        public async Task<Result<bool>> AddKirana(KiranaDto dto)
        {
            using (var transaction = this.Db.Database.BeginTransaction())
            {
                try
                {
                    dto.Addon = DateTime.Now;
                    dto.KiranaCode = KiranaCode();
                    dto.ContactPerson = "NA";
                    // get all roles
                    var roleList = await this.UserService.Roles();

                    // Get the role ID for "Tenant" role
                    var roleId = roleList.Entity
                                   .Where(x => x.Name == ApplicationUserRole.Kirana.ToString())
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
                                                new ApplicationUserRoleDto(){ Id = roleId, Name = ApplicationUserRole.Kirana.ToString() }
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


                    var result = _mapper.Map<Kirana>(dto);
                    result.UserId = applicationUser.Id;

                    // Add the SalesPerson to the DbContext's SalesPerson DbSet.
                    await this.Db.Kirana.AddAsync(result);

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
        public string KiranaCode()
        {

            var data = Db.Kirana.ToList().Count;


            string data2 = "";
            if (data == 0)
            {
                int id = 1;


                data2 = "Kirana-" + id;
            }
            else
            {
                int id = data + 1;
                data2 = "Kirana-" + id;
            }
            return data2;
        }
        public async Task<Result<bool>> DeleteKirana(int id)
        {
            try
            {
                if (id == 0)
                {
                    return Result<bool>.NotFound();
                }

                // need to implement
                //  if sales person associate with other group,then we can not delete untill we discontinue sales person in other group
                var Kirana = await this.Db.Kirana.Where(x => x.Id == id).SingleOrDefaultAsync();
                var applicationUser = await this.Db.Users.Where(a => a.Id == id).SingleOrDefaultAsync();
                if (applicationUser != null)
                {
                    bool res = UserService.DeleteApplicationUser(applicationUser.Id);
                }
                this.Db.Kirana.Remove(Kirana);
                await this.Db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error in Deleting Kirana");
            }
        }

        public async Task<Result<KiranaDto>> GetKirana(int id)
        {
            try
            {
                var Partner = await this.Db.Kirana.Where(x => x.Id == id).SingleOrDefaultAsync();

                var result = _mapper.Map<KiranaDto>(Partner);
                return Result<KiranaDto>.Success(result);
            }
            catch (Exception ex) { return Result<KiranaDto>.Failure("Error in getting Sales Kirana"); }
        }

        public async Task<Result<bool>> UpdateKirana(KiranaDto dto)
        {
            try
            {

                var Kirana = _mapper.Map<Kirana>(dto);

                this.Db.Kirana.Attach(Kirana);
                this.Db.Entry(Kirana).State = EntityState.Modified;

                await this.Db.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error in Updating Kirana");
            }
        }


        public List<Partner> partners()
        {
            var data = Db.Partner.ToList();
            return data;
        }

        public async Task<Result<List<Partner>>> GetpartnerList()
        {
            try
            {
                var data = await Db.Partner.ToListAsync();
                return Result<List<Partner>>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<List<Partner>>.Failure("Error in getting Partner");
            }
        }

        public async Task<Result<List<Kirana>>> GetKiranaDetailsList()
        {
            try
            {
                var data = await Db.Kirana.ToListAsync();
                return Result<List<Kirana>>.Success(data);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error result
                return Result<List<Kirana>>.Failure("Failed to retrieve salespersons: " + ex.Message);
            }
        }



    }
}
