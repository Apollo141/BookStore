using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record AuthDto(
        [Required(ErrorMessage = "The Username field is required.")]
        [StringLength(50, ErrorMessage = "The Username must be at most 50 characters long.")]
        string Username,

        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(100, ErrorMessage = "The Password must be at most 100 characters long.")]
        string Password
    );
}
