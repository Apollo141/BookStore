using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record OrderItemDto(
        [Required(ErrorMessage = "The ProductId field is required.")]
        int ProductId,

        [Required(ErrorMessage = "The Quantity field is required.")]
        [Range(1, 100, ErrorMessage = "The Quantity must be between 1 and 100.")]
        int Quantity,

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0.01, 10000.00, ErrorMessage = "The Price must be between 0.01 and 10,000.00.")]
        decimal Price
    );
}
