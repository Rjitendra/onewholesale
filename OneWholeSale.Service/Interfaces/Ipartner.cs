using OneWholeSale.Model.Dto.Partner;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Model.Entity.Partner;
using OneWholeSale.Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Service.Interfaces
{
    public interface Ipartner
    {
        Task<Result<PartnerDto>> GetPartner(int id);
        Task<Result<bool>> AddPartner(PartnerDto dto);
        Task<Result<bool>> UpdatePartner(PartnerDto dto);
        Task<Result<bool>> DeletePartner(int id);
    }
}
