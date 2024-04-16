namespace Application.DTOs
{
    /// <summary>
    /// گرفتن صفحه بندی جهت نمایش گرید یک موجودیت خاص 
    /// </summary>
    /// <typeparam name="T">نوع موجودیت</typeparam>
    public class ItemListDTO<T> : IItemListDTO<T> where T : class
    {
        #region Constructors
        public ItemListDTO() { }

        public ItemListDTO(int page, int pageLength, int totalCount, int filteredCount, List<T> items)
        {
            Page = page;
            PageLength = pageLength;
            TotalCount = totalCount;
            Items = items;
            FilteredCount = filteredCount;
        }
        #endregion


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
        /// تعداد کل آیتم ها بدون فیلتر
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// تعداد کل آیتم های فیلتر شده
        /// </summary>
        public int FilteredCount { get; set; }

        /// <summary>
        /// تعداد صفحات
        /// </summary>
        public int PageCount
        {
            get
            {
                var count = FilteredCount / PageLength;
                if (FilteredCount % PageLength > 0)
                    count++;
                return count;
            }
        }

        /// <summary>
        /// لیست آیتم ها
        /// </summary>
        public List<T> Items { get; set; } 
        #endregion
    }
}
