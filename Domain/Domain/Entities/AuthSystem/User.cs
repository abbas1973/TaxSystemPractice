using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity, IIsEnabled
    {
        #region Constructors
        public User() : base()
        {
            IsEnabled = true;
        }
        #endregion


        #region Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long CityId { get; set; }
        public City City { get; set; }
        public long? CompanyId { get; set; }
        public Company Company { get; set; }
        public bool IsEnabled { get; set; }
        public bool MobileConfirmed { get; set; }
        public bool PasswordIsChange { get; set; }
        #endregion


        #region Navigation Properties
        /// <summary>
        /// نقش های کاربر
        /// </summary>
        public ICollection<UserRole> Roles { get; set; }
        #endregion
    }
}
