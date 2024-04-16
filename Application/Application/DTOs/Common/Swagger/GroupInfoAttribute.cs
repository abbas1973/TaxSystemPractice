using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Swagger
{
    public class GroupInfoAttribute : Attribute
    {
        /// <summary>
        /// عنوان گروه برای نمایش در دراپدون
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// توضیحات این گروه از وبسرویس ها
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ورژن
        /// </summary>
        public string Version { get; set; }
    }
}
