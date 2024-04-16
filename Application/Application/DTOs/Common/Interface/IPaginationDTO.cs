using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public interface IPaginationDTO
    {
        /// <summary>
        /// شماره صفحه
        /// </summary>
        [Display(Name = "شماره صفحه")]
        int? Page { get; set; }


        /// <summary>
        /// تعداد آیتم های درون صفحه
        /// </summary>
        [Display(Name = "تعداد آیتم های صفحه")]
        int? PageLength { get; set; }
    }
}
