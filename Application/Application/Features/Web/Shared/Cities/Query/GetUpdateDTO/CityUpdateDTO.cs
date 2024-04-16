using Application.DTOs;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.Features.Web.Cities
{
    public class CityUpdateDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "استان")]
        public long ProvinceId { get; set; }

        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<City, CityUpdateDTO>> Selector
        {
            get
            {
                return model => new CityUpdateDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                    ProvinceId = model.ProvinceId,
                    IsEnabled = model.IsEnabled,
                };
            }
        }
        #endregion
    }
}
