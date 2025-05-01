using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record OrderDto(
        [Required(ErrorMessage = "The OrderId field is required.")]
        int OrderId,

        [Required(ErrorMessage = "The CustomerId field is required.")]
        int CustomerId,

        [Required(ErrorMessage = "The OrderDate field is required.")]
        DateTime OrderDate,

        [Required(ErrorMessage = "The TotalAmount field is required.")]
        [Range(0.01, 100000.00, ErrorMessage = "The TotalAmount must be between 0.01 and 100,000.00.")]
        decimal TotalAmount,

        [Required(ErrorMessage = "The Items field is required.")]
        List<OrderItemDto> Items
    );
}
