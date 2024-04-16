using Application.Contracts;
using DTOs.DataTable;
using Microsoft.AspNetCore.Http;



namespace Infrastructure.Implementation
{
    public class DataTableManager : IDataTableManager
    {
        IHttpContextAccessor httpContextAccessor;
        public DataTableManager(IHttpContextAccessor _httpContextAccessor) 
        {
            httpContextAccessor = _httpContextAccessor;
        }



        /// <summary>
        /// گرفتن مدل لازم برای سرچ
        /// </summary>
        /// <returns></returns>
        public DataTableSearchDTO GetSearchModel()
        {
            var Request = httpContextAccessor.HttpContext.Request;
            var model = new DataTableSearchDTO
            {
                start = Convert.ToInt32(Request.Form["start"]),
                length = Convert.ToInt32(Request.Form["length"]),
                searchValue = Request.Form["search[value]"],
                sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"] + "][data]"],
                sortDirection = Request.Form["order[0][dir]"],
                draw = Request.Form["draw"]
            };
            return model;
        }


    }
}
