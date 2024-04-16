using Application.DTOs;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.Features.Web.Companies
{
    public class CompanyDataTableDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }

        [Display(Name = "شناسه ملی")]
        public string NationalCode { get; set; }

        /// <summary>
        /// شناسه اقتصادی شرکت
        /// </summary>
        [Display(Name = "شناسه اقتصادی")]
        public string EconomicCode { get; set; }

        /// <summary>
        /// کلاینت آیدی از سامانه مودیان
        /// </summary>
        [Display(Name = "کلاینت آیدی سامانه مودیان")]
        public string ClientId { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<Company, CompanyDataTableDTO>> Selector
        {
            get
            {
                return model => new CompanyDataTableDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                    EconomicCode = model.EconomicCode,
                    NationalCode = model.NationalCode,
                    ClientId = model.ClientId
                };
            }
        }
        #endregion
    }
}
