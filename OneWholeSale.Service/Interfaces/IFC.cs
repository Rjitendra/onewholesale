using OneWholeSale.Model.Dto.FC;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Service.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneWholeSale.Service.Interfaces
{
    public interface IFC
    {

         Task<Result<bool>> DeleteFc(int id);
        Task<Result<bool>> AddNewFC(FullFillmentCenterDto dto);
    }
}
