using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public interface IUsernameDTO
    {
        [Display(Name = "نام کاربری")]
        string Username { get; set; }
    }
}
