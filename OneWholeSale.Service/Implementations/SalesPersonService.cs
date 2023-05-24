namespace OneWholeSale.Service.Implementations
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using OneWholeSale.Model.Context;
    using OneWholeSale.Model.Dto;
    using OneWholeSale.Model.Dto.SalesPerson;
    using OneWholeSale.Model.Entity.Master;
    using OneWholeSale.Model.Entity.SalesPerson;
    using OneWholeSale.Model.Enums;
    using OneWholeSale.Service.Interfaces;
    using OneWholeSale.Service.Utility;
    using System.Threading.Tasks;

    public class SalesPersonService : ISalesPersonService
    {
        private ApiContext Db { get; set; }


        private IMapper _mapper;
        private CurrentUser CurrentUser { get; set; }
        private IUserService UserService { get; }
        public SalesPersonService(
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
        public async Task<Result<SalesPersonDto>> GetSalesPerson(int id)
        {
            try
            {
                var salesPerson = await this.Db.SalesPerson.Where(x => x.Id == id).SingleOrDefaultAsync();

                var result = _mapper.Map<SalesPersonDto>(salesPerson);
                return Result<SalesPersonDto>.Success(result);
            }
            catch (Exception ex) { return Result<SalesPersonDto>.Failure("Error in getting Sales Person"); }
        }
        public async Task<Result<bool>> AddSalesPerson(SalesPersonDto dto)
        {
            using (var transaction = this.Db.Database.BeginTransaction())
            {
                try
                {
                    dto.AddOn = DateTime.Now;
                    dto.SalesPersonCode = SalesPerson_Code();
                    // get all roles
                    var roleList = await this.UserService.Roles();

                    // Get the role ID for "Tenant" role
                    var roleId = roleList.Entity
                                   .Where(x => x.Name == ApplicationUserRole.SalesPerson.ToString())
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
                                                new ApplicationUserRoleDto(){ Id = roleId, Name = ApplicationUserRole.SalesPerson.ToString() }
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


                    var result = _mapper.Map<SalesPerson>(dto);
                    result.UserId = applicationUser.Id;

                    // Add the SalesPerson to the DbContext's SalesPerson DbSet.
                    await this.Db.SalesPerson.AddAsync(result);

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
                        return Result<bool>.Failure("Failed to create Sales Person");
                    }
                    return Result<bool>.Failure("Failed to create Sales Person");
                }
            }
        }

        public async Task<Result<bool>> DeleteSalesPerson(int id)
        {
            try
            {
                if (id == 0)
                {
                    return Result<bool>.NotFound();
                }

                // need to implement
                //  if sales person associate with other group,then we can not delete untill we discontinue sales person in other group
                var salesPerson = await this.Db.SalesPerson.Where(x => x.Id == id).SingleOrDefaultAsync();
                var applicationUser = await this.Db.Users.Where(a => a.Id == id).SingleOrDefaultAsync();
                if (applicationUser != null)
                {
                    bool res = UserService.DeleteApplicationUser(applicationUser.Id);
                }
                this.Db.SalesPerson.Remove(salesPerson);
                await this.Db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex) { 
                return Result<bool>.Failure("Error in Deleting Sales Person"); 
            }
        }
        public async Task<Result<bool>> UpdateSalesPerson(SalesPersonDto dto)
        {
            try
            {

                var salesPerson = _mapper.Map<SalesPerson>(dto);

                this.Db.SalesPerson.Attach(salesPerson);
                this.Db.Entry(salesPerson).State = EntityState.Modified;

                await this.Db.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error in Updating Sales Person");
            }
        }


        public string SalesPerson_Code()
        {

            var data = Db.SalesPerson.ToList().Count ;


            string data2 = "";
            if (data == 0)
            {
                int id = 1;


                data2 = "SalesPerson-" + id;
            }
            else
            {
                int id = data + 1;
                data2 = "SalesPerson-" + id;
            }
            return data2;
        }

        public List<District> Districtlist()
        {
            var data = Db.District.ToList();

            return data;
        }

        public async Task<Result<List<District>>> GetDistrict_List()
        {
            try
            {
                var data = await Db.District.ToListAsync();
                return Result<List<District>>.Success(data);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error result
                return Result<List<District>>.Failure("Failed to retrieve salespersons: " + ex.Message);
            }
        }



        public List<Vw_SalesPerson> SalesPersonList()
        {
            var data = Db.Vw_SalesPerson.ToList();

            return data;
        }
        public async Task<Result<List<Vw_SalesPerson>>> GetSalesPersonsonList()
        {
            try
            {
                var data = await Db.Vw_SalesPerson.ToListAsync();
                return Result<List<Vw_SalesPerson>>.Success(data);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error result
                return Result<List<Vw_SalesPerson>>.Failure("Failed to retrieve salespersons: " + ex.Message);
            }
        }
    }
}
