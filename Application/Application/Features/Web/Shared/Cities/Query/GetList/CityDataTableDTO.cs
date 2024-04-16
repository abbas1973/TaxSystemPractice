using Application.DTOs;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.Features.Web.Cities
{
    public class CityDataTableDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "شناسه استان")]
        public long ProvinceId { get; set; }

        [Display(Name = "استان")]
        public string Province { get; set; }

        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<City, CityDataTableDTO>> Selector
        {
            get
            {
                return model => new CityDataTableDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                    IsEnabled = model.IsEnabled,
                    ProvinceId = model.ProvinceId,
                    Province = model.Province.Name,
                };
            }
        }
        #endregion
    }
}
