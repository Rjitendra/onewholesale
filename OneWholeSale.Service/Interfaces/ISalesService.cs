namespace OneWholeSale.Service.Interfaces
{
    using OneWholeSale.Model.Dto.SalesPerson;
    using OneWholeSale.Service.Utility;
    public interface ISalesService
    {
        Task<Result<SalesPersonDto>> GetSalesPerson(int id);
        Task<Result<bool>> AddSalesPerson(SalesPersonDto dto);
        Task<Result<bool>> UpdateSalesPerson(SalesPersonDto dto);
        Task<Result<bool>> DeleteSalesPerson(int id);
   }
}
