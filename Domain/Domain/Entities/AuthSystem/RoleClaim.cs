using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RoleClaim : BaseEntity
    {
        #region Properties
        public long RoleId { get; set; }
        public Role Role { get; set; }
        public string Claim { get; set; }
        #endregion


    }
}
