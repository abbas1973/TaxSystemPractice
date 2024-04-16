using Application.DTOs;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.Features.Web.Provinces
{
    public class ProvinceDataTableDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }


        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<Province, ProvinceDataTableDTO>> Selector
        {
            get
            {
                return model => new ProvinceDataTableDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                    IsEnabled = model.IsEnabled,
                };
            }
        }
        #endregion
    }
}
