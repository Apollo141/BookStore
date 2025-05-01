using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId);
        Task<OrderDto> GetOrderAsync(int orderId);
        Task<List<OrderDto>> GetUserOrdersAsync(string userId);
    }
}
