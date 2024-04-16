using Application.Features.Web.Users;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.DTOs
{
    /// <summary>
    /// مدل برای دراپدون
    /// </summary>
    public class SelectListDTO
    {
        #region Constructors
        public SelectListDTO() { }

        public SelectListDTO(long id, string title)
        {
            Id = id;
            Title = title;
        }
        #endregion



        #region Properties
        /// <summary>
        /// شناسه
        /// </summary>
        [Display(Name = "شناسه")]
        public long Id { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>

        [Display(Name = "عنوان")]
        public string Title { get; set; }
        #endregion




        #region سلکتور ها
        #region Role => SelectListDTO
        public static Expression<Func<Role, SelectListDTO>> RoleSelector
        {
            get
            {
                return model => new SelectListDTO()
                {
                    Id = model.Id,
                    Title = model.Title,
                };
            }
        }
        #endregion


        #region UserRole => SelectListDTO
        public static Expression<Func<UserRole, SelectListDTO>> UserRoleSelector
        {
            get
            {
                return model => new SelectListDTO()
                {
                    Id = model.RoleId,
                    Title = model.Role.Title,
                };
            }
        }
        #endregion


        #region Province => SelectListDTO
        public static Expression<Func<Province, SelectListDTO>> ProvinceSelector
        {
            get
            {
                return model => new SelectListDTO()
                {
                    Id = model.Id,
                    Title = model.Name,
                };
            }
        }
        #endregion


        #region City => SelectListDTO
        public static Expression<Func<City, SelectListDTO>> CitySelector
        {
            get
            {
                return model => new SelectListDTO()
                {
                    Id = model.Id,
                    Title = model.Name,
                };
            }
        }
        #endregion
        #endregion
    }
}
