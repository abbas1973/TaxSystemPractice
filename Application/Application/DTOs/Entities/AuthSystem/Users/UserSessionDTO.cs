using Domain.Entities;
using System.Linq.Expressions;

namespace Application.DTOs.Users
{
    public class UserSessionDTO : BaseEntityDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName} {LastName}".Trim();
        public string UserName { get; set; }
        public long? CompanyId { get; set; }
        public string Company { get; set; }
        public bool IsEnabled { get; set; }
        public bool MobileConfirmed { get; set; }
        public bool PasswordIsChanged { get; set; }



        #region User => UserSessionDTO
        public static Expression<Func<User, UserSessionDTO>> Selector
        {
            get
            {
                return model => new UserSessionDTO()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    IsEnabled = model.IsEnabled,
                    MobileConfirmed = model.MobileConfirmed,
                    PasswordIsChanged = model.PasswordIsChange,
                    CompanyId = model.CompanyId,
                    Company = model.Company.Name
                };
            }
        }
        #endregion
    }
}
