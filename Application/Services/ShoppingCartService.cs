using Application.DTOs;
using Application.Interfaces;
using Infastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IApplicationDbContext _ctx;
        private readonly IOrderService _orderSvc;

        public ShoppingCartService(IApplicationDbContext ctx, IOrderService orderSvc)
        {
            _ctx = ctx;
            _orderSvc = orderSvc;
        }

        public async Task AddItemAsync(string userId, int bookId, int qty)
        {
            var cart = await GetOrCreateCartAsync(userId);

            var item = cart.Items.FirstOrDefault(i => i.BookId == bookId);
            if (item == null)
            {
                item = new ShoppingCartItem
                {
                    BookId = bookId,
                    Quantity = qty,
                    ShoppingCartId = cart.Id
                };
                _ctx.ShoppingCartItems.Add(item);
            }
            else
            {
                item.Quantity += qty;
            }

            await _ctx.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(string userId, int bookId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var item = cart.Items.FirstOrDefault(i => i.BookId == bookId);
            if (item != null)
            {
                _ctx.ShoppingCartItems.Remove(item);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _ctx.ShoppingCarts
                .Include(c => c.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return new CartDto(userId, new());

            var items = cart.Items
                .Select(i => new CartItemDto(i.BookId, i.Quantity))
                .ToList();

            return new CartDto(userId, items);
        }

        public async Task CheckoutAsync(string userId)
        {
            // Create the order
            var orderDto = await _orderSvc.CreateOrderAsync(userId);

            // Clear the cart
            var cart = await GetOrCreateCartAsync(userId);
            _ctx.ShoppingCartItems.RemoveRange(cart.Items);
            await _ctx.SaveChangesAsync();
        }

        private async Task<ShoppingCart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _ctx.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    Items = new List<ShoppingCartItem>()
                };
                _ctx.ShoppingCarts.Add(cart);
                await _ctx.SaveChangesAsync();
            }

            return cart;
        }
    }
}
