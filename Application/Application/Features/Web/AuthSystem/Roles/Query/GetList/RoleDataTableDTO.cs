using Application.DTOs;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.Features.Web.Roles
{
    public class RoleDataTableDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<Role, RoleDataTableDTO>> Selector
        {
            get
            {
                return model => new RoleDataTableDTO()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    IsEnabled = model.IsEnabled
                };
            }
        }
        #endregion
    }
}
