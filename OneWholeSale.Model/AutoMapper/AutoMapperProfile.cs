using AutoMapper;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Model.Entity.SalesPerson;

namespace OneWholeSale.Model.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SalesPerson, SalesPersonDto>().ReverseMap();
        }
    }
}
