using DTOs.DataTable;

namespace Application.Contracts
{
    public interface IDataTableManager
    {
        /// <summary>
        /// گرفتن مدل لازم برای سرچ در دیتاتیبل
        /// </summary>
        DataTableSearchDTO GetSearchModel();

    }
}
