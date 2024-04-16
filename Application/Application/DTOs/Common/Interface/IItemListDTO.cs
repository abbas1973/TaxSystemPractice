namespace Application.DTOs
{
    /// <summary>
    /// گرفتن صفحه بندی جهت نمایش گرید یک موجودیت خاص 
    /// </summary>
    /// <typeparam name="T">نوع موجودیت</typeparam>
    public interface IItemListDTO<T> where T : class
    {
        #region Properties
        /// <summary>
        /// شماره صفحه
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// تعداد آیتم ها در هر صفحه
        /// </summary>
        int PageLength { get; set; }

        /// <summary>
        /// تعداد کل آیتم ها متناسب با فیلتر
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// تعداد صفحات
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// لیست آیتم ها
        /// </summary>
        List<T> Items { get; set; } 
        #endregion
    }
}
