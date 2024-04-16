using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : BaseEntity, IIsEnabled
    {
        #region Properties
        public string Title { get; set; }
        public bool IsEnabled { get; set; }
        public string Description { get; set; }
        #endregion



        #region Navigation Properties
        /// <summary>
        /// کاربرانی که این نقش را دارند
        /// </summary>
        public ICollection<UserRole> Users { get; set; }
        public ICollection<RoleClaim> Claims { get; set; }
        #endregion
    }
}
