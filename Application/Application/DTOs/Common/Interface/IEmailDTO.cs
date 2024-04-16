using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public interface IEmailDTO
    {
        [Display(Name = "ایمیل")]
        string Email { get; set; }
    }
}
