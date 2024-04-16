using Application.DTOs;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.Features.Web.Profiles
{
    public class UserProfileUpdateDTO : BaseEntityDTO
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

        [Display(Name = "شهر")]
        public string City { get; set; }

        [Display(Name = "شناسه استان")]
        public long ProvinceId { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<User, UserProfileUpdateDTO>> Selector
        {
            get
            {
                return model => new UserProfileUpdateDTO()
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.Mobile,
                    CityId = model.CityId,
                    City = model.City.Name,
                    ProvinceId = model.City.ProvinceId,
                    Username = model.Username,
                };
            }
        }
        #endregion
    }
}
