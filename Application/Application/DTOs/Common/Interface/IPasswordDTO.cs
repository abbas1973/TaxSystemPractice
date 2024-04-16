using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public interface IPasswordDTO
    {
        [Display(Name = "کلمه عبور")]
        string Password { get; set; }
    }
}
