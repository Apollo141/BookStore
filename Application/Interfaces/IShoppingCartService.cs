using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddItemAsync(string userId, int bookId, int qty);
        Task RemoveItemAsync(string userId, int bookId);
        Task<CartDto> GetCartAsync(string userId);
        Task CheckoutAsync(string userId);
    }
}
