using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record BookDto(
        [Required(ErrorMessage = "The Id field is required.")]
        int Id,

        [Required(ErrorMessage = "The Title field is required.")]
        [StringLength(100, ErrorMessage = "The Title must be at most 100 characters long.")]
        string Title,

        [Required(ErrorMessage = "The Author field is required.")]
        [StringLength(50, ErrorMessage = "The Author must be at most 50 characters long.")]
        string Author,

        [Required(ErrorMessage = "The Genre field is required.")]
        [StringLength(30, ErrorMessage = "The Genre must be at most 30 characters long.")]
        string Genre,

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0.01, 10000.00, ErrorMessage = "The Price must be between 0.01 and 10,000.00.")]
        decimal Price,

        [Required(ErrorMessage = "The IsAvailable field is required.")]
        bool IsAvailable
    );
}
