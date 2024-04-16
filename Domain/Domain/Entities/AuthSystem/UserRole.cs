﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserRole : BaseEntity
    {
        #region Properties
        public long UserId { get; set; }
        public User User { get; set; }
        public long RoleId { get; set; }
        public Role Role { get; set; }
        #endregion

    }
}
