using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record OrderDto(
     int Id,
     string UserId,
     DateTime OrderDate,
     decimal TotalAmount,
     List<OrderItemDto> Items
 );
}
