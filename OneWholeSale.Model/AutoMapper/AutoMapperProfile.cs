﻿using AutoMapper;
using OneWholeSale.Model.Dto;
using OneWholeSale.Model.Dto.Partner;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Model.Entity.Kirana;
using OneWholeSale.Model.Entity.Partner;
using OneWholeSale.Model.Entity.SalesPerson;

namespace OneWholeSale.Model.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SalesPerson, SalesPersonDto>().ReverseMap();
            CreateMap<Partner, PartnerDto>().ReverseMap();
            CreateMap<Kirana, KiranaDto>().ReverseMap();
        }
    }
}
