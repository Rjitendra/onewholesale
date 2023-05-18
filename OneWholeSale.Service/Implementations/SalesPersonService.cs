namespace OneWholeSale.Service.Implementations
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using OneWholeSale.Model.Context;
    using OneWholeSale.Model.Dto.SalesPerson;
    using OneWholeSale.Service.Interfaces;
    using OneWholeSale.Service.Utility;
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
        public Task<Result<bool>> AddSalesPerson(SalesPersonDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteSalesPerson(int id)
        {
            throw new NotImplementedException();
        }
        public Task<Result<bool>> UpdateSalesPerson(SalesPersonDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
