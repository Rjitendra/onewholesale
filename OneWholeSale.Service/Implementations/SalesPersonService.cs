namespace OneWholeSale.Service.Implementations
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using OneWholeSale.Model.Context;
    using OneWholeSale.Model.Dto.SalesPerson;
    using OneWholeSale.Model.Entity.SalesPerson;
    using OneWholeSale.Service.Interfaces;
    using OneWholeSale.Service.Utility;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class SalesPersonService : ISalesPersonService
    {
        private ApiContext Db { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public SalesPersonService(
             ApiContext db,
            IHttpContextAccessor httpContextAccessor,
             IMapper mapper
           )
        {
            this.Db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<SalesPersonDto>> GetSalesPerson(int id)
        {
            try
            {
                var salesPerson = this.Db.SalesPerson.Where(x => x.Id == id).SingleOrDefaultAsync();

                var result = _mapper.Map<SalesPersonDto>(salesPerson);
                return Result<SalesPersonDto>.Success(result);
            }
            catch (Exception ex) { return Result<SalesPersonDto>.Failure("Error in getting Sales Person"); }
        }
        public async Task<Result<bool>> AddSalesPerson(SalesPersonDto dto)
        {
            try
            {
                var result = _mapper.Map<SalesPerson>(dto);
                // Add the ticket to the DbContext's Ticket DbSet.
                await this.Db.SalesPerson.AddAsync(result);

                // Save the changes to the database.
                await this.Db.SaveChangesAsync();

                // Return a Result object indicating success and a value of true.
                return Result<bool>.Success(true);
            }
            catch (Exception ex) { return Result<bool>.Failure("Error in Adding Sales Person"); }
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
                this.Db.SalesPerson.Remove(salesPerson);
                await this.Db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex) { return Result<bool>.Failure("Error in Deleting Sales Person"); }
        }
        public async Task<Result<bool>> UpdateSalesPerson(SalesPersonDto dto)
        {

            try
            {
                var salesPerson = _mapper.Map<SalesPerson>(dto);
                
               
                await this.Db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Error in Updating Sales Person");
            }
        }
    }
}
