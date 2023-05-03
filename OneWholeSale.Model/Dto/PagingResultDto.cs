namespace OneWholeSale.Model.Dto
{
    using System.Collections.Generic;

    public class PagingResultDto<T>
    {
        /// <summary>
        /// Property to hold the total row count in result
        /// </summary>
        public int TotalRowCount { get; set; }
        /// <summary>
        /// Property that will contain the result
        /// </summary>
        public IEnumerable<T> PagingResult { get; set; }
    }
}