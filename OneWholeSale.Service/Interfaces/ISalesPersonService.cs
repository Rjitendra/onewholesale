namespace OneWholeSale.Service.Interfaces
{
    using OneWholeSale.Model.Dto.SalesPerson;
    using OneWholeSale.Model.Entity.Master;
    using OneWholeSale.Model.Entity.SalesPerson;
    using OneWholeSale.Service.Utility;
    public interface ISalesPersonService
    {
        Task<Result<SalesPersonDto>> GetSalesPerson(int id);
        Task<Result<bool>> AddSalesPerson(SalesPersonDto dto);
        Task<Result<bool>> UpdateSalesPerson(SalesPersonDto dto);
        Task<Result<bool>> DeleteSalesPerson(int id);

        Task<Result<List<District>>> GetDistrict_List();
        Task<Result<List<Vw_SalesPerson>>> GetSalesPersonsonList();

    }
}