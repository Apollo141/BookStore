using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record CartDto(
        [Required(ErrorMessage = "The CartId field is required.")]
        int CartId,

        [Required(ErrorMessage = "The UserId field is required.")]
        int UserId,

        [Required(ErrorMessage = "The Items field is required.")]
        List<CartItemDto> Items
    );
}
