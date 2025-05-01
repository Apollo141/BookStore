using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record LoginDto(
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field must be a valid email address.")]
        string Email,

        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(100, ErrorMessage = "The Password must be at most 100 characters long.")]
        string Password
    );
}
