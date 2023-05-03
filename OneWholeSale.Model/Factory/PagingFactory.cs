namespace OneWholeSale.Model.Factory
{
    using OneWholeSale.Model.Dto;
    public class PagingFactory
    {
        public static PagingResultDto<T> Create<T>(IEnumerable<T> model, int totalRecords) where T : class
        {
            return new PagingResultDto<T> { PagingResult = model, TotalRowCount = totalRecords };
        }
    }
}
