using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Swagger
{
    /// <summary>
    /// System grouping characteristics
    /// </summary>
    public class ApiGroupAttribute : Attribute
    {
        public ApiGroupAttribute(params ApiGroupNames[] name)
        {
            GroupName = name;
        }
        public ApiGroupNames[] GroupName { get; set; }
    }
}
