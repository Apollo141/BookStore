using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record UserDto(
        [Required(ErrorMessage = "The UserId field is required.")]
        int UserId,

        [Required(ErrorMessage = "The Username field is required.")]
        [StringLength(50, ErrorMessage = "The Username must be at most 50 characters long.")]
        string Username,

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field must be a valid email address.")]
        string Email
    );
}
