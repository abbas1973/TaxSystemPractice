namespace Application.DTOs
{
    #region Implementation
    /// <summary>
    /// جستجو روی گرید
    /// </summary>
    public abstract class GridSearchDTO : IGridSearchDTO
    {
        #region Constructors
        public GridSearchDTO() { }

        public GridSearchDTO(
            int page = 1,
            int pageLength = 10,
            string sortColumn = "id",
            string sortDirection = "desc")
        {
            Page = page;
            PageLength = pageLength;
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }
        #endregion


        #region Properties
        /// <summary>
        /// شماره صفحه
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// تعداد آیتم ها در هر صفحه
        /// </summary>
        public int PageLength { get; set; } = 10;

        /// <summary>
        /// نقطه شروع صفحه بندی
        /// </summary>
        public int Start => (Page - 1) * PageLength;

        /// <summary>
        /// مرتب سازی بر اساس کدام ستون داده باشد؟
        /// </summary>
        public string SortColumn { get; set; } = "Id";

        /// <summary>
        /// جهت مرتب سازی
        /// <para>
        /// asc = صعودی
        /// </para>
        /// <para>
        /// desc = نزولی
        /// </para>
        /// </summary>
        public string SortDirection { get; set; } = "desc";
        #endregion
    } 
    #endregion




    #region interface
    public interface IGridSearchDTO
    {
        #region Properties
        /// <summary>
        /// شماره صفحه
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// تعداد آیتم ها در هر صفحه
        /// </summary>
        public int PageLength { get; set; }

        /// <summary>
        /// نقطه شروع صفحه بندی
        /// </summary>
        public int Start { get; }


        /// <summary>
        /// مرتب سازی بر اساس کدام ستون داده باشد؟
        /// </summary>
        public string SortColumn { get; set; }

        /// <summary>
        /// جهت مرتب سازی
        /// <para>
        /// asc = صعودی
        /// </para>
        /// <para>
        /// desc = نزولی
        /// </para>
        /// </summary>
        public string SortDirection { get; set; }
        #endregion
    } 
    #endregion
}
