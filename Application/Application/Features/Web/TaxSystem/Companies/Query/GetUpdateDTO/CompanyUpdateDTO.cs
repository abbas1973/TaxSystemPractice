using Application.DTOs;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Application.Features.Web.Companies
{
    public class CompanyUpdateDTO : BaseEntityDTO
    {
        #region Properties
        [Display(Name = "نام")]
        public string Name { get; set; }

        /// <summary>
        /// کلید اتصال به سامانه مودیان
        /// </summary>
        [Display(Name = "کلید اختصاصی سامانه مودیان")]
        public string PrivateKey { get; set; }


        /// <summary>
        /// شناسه اقتصادی شرکت
        /// </summary>
        [Display(Name = "شناسه اقتصادی")]
        public string EconomicCode { get; set; }

        [Display(Name = "شناسه ملی")]
        public string NationalCode { get; set; }

        /// <summary>
        /// کلاینت آیدی از سامانه مودیان
        /// </summary>
        [Display(Name = "کلاینت آیدی سامانه مودیان")]
        public string ClientId { get; set; }
        #endregion


        #region سلکتور
        public static Expression<Func<Company, CompanyUpdateDTO>> Selector
        {
            get
            {
                return model => new CompanyUpdateDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                    ClientId = model.ClientId,
                    EconomicCode = model.EconomicCode,
                    NationalCode = model.NationalCode,
                    PrivateKey = model.PrivateKey
                };
            }
        }
        #endregion
    }
}
