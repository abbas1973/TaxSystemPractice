using Application.DTOs;
using Application.Features.Web.Users;
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
    public class UserUpdateDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Display(Name = "موبایل")]
        public string Mobile { get; set; }

        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Display(Name = "شناسه شهر")]
        public long CityId { get; set; }

        [Display(Name = "شناسه استان")]
        public long ProvinceId { get; set; }

        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }

        [Display(Name = "نقش ها")]
        public List<long> RoleIds { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<User, UserUpdateDTO>> Selector
        {
            get
            {
                return model => new UserUpdateDTO()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.Mobile,
                    IsEnabled = model.IsEnabled,
                    CityId = model.CityId,
                    ProvinceId = model.City.ProvinceId,
                    Username = model.Username,
                    RoleIds = model.Roles.Select(x => x.RoleId).ToList()
                };
            }
        }
        #endregion
    }
}
