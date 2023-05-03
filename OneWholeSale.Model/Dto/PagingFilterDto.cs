namespace OneWholeSale.Model.Dto
{
    public class PagingFilterDto
    {
        /// <summary>
        /// Property to hold current page index
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Property to hold Page Size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Property to hold sorting direction asc or desc
        /// </summary>
        public string SortDirection { get; set; }

        /// <summary>
        /// property to hold Grid Sort Column name
        /// </summary>
        public string SortName { get; set; }
    }
}