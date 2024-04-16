using Application.DTOs;
using Application.Features.Web.Roles;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Web.Roles
{
    public class RoleUpdateDTO : BaseEntityDTO
    {

        #region Properties
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "فعال/غیرفعال")]
        public bool IsEnabled { get; set; }

        [Display(Name = "دسترسی ها")]
        public List<string> Claims { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<Role, RoleUpdateDTO>> Selector
        {
            get
            {
                return model => new RoleUpdateDTO()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    IsEnabled = model.IsEnabled,
                    Claims = model.Claims.Select(x => x.Claim).ToList()
                };
            }
        }
        #endregion
    }
}
