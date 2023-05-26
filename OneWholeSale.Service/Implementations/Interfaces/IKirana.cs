using OneWholeSale.Model.Dto;
using OneWholeSale.Model.Dto.Partner;
using OneWholeSale.Model.Entity.Kirana;
using OneWholeSale.Model.Entity.Partner;
using OneWholeSale.Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Service.Implementations.Interfaces
{
    public interface IKirana
    {
        Task<Result<KiranaDto>> GetKirana(int id);
        Task<Result<bool>> AddKirana(KiranaDto dto);
        Task<Result<bool>> UpdateKirana(KiranaDto dto);
        Task<Result<bool>> DeleteKirana(int id);
        Task<Result<List<Partner>>> GetpartnerList();
        Task<Result<List<Kirana>>> GetKiranaDetailsList();
    }
}
