using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public interface IMobileDTO
    {
        [Display(Name = "تلفن همراه")]
        string Mobile { get; set; }
    }
}
