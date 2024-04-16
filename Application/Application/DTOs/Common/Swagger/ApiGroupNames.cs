using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Swagger
{
    /// <summary>
    /// System group enumeration value
    /// </summary>
    public enum ApiGroupNames
    {
        [GroupInfo(Title = "همه", Description = "همه وب سرویس ها", Version = "v1")]
        All = 0,

        [GroupInfo(Title = "احراز هویت ادمین", Description = "احراز هویت ادمین", Version = "v1")]
        AuthAdmin_V1 = 1,

        [GroupInfo(Title = "اطلاعات پایه", Description = "اطلاعات پایه", Version = "v1")]
        Shared_V1 = 2,

        [GroupInfo(Title = "سیستم مالی", Description = "سیستم مالی", Version = "v1")]
        FinancialSystem_V1 = 3


    }
}
