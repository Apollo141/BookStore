using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record CartItemDto(
        [Required(ErrorMessage = "The ProductId field is required.")]
        int ProductId,

        [Required(ErrorMessage = "The Quantity field is required.")]
        [Range(1, 100, ErrorMessage = "The Quantity must be between 1 and 100.")]
        int Quantity
    );
}
