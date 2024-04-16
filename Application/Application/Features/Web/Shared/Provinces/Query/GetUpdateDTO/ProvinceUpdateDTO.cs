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

namespace Application.Features.Web.Provinces
{
    public class ProvinceUpdateDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }


        [Display(Name = "فعال")]
        public bool IsEnabled { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<Province, ProvinceUpdateDTO>> Selector
        {
            get
            {
                return model => new ProvinceUpdateDTO()
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
