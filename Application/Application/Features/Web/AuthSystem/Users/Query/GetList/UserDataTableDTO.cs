using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Web.Users
{
    public class UserDataTableDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "نام کامل")]
        public string Name => $"{FirstName} {LastName}".Trim();

        [Display(Name = "موبایل")]
        public string Mobile { get; set; }

        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "شناسه شهر")]
        public long CityId { get; set; }

        [Display(Name = "شهر")]
        public string City { get; set; }

        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<User, UserDataTableDTO>> Selector
        {
            get
            {
                return model => new UserDataTableDTO()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.Mobile,
                    IsEnabled = model.IsEnabled,
                    CityId = model.CityId,
                    City = model.City.Name,
                    Username = model.Username
                };
            }
        }
        #endregion
    }
}
