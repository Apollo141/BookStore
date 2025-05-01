using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IApplicationDbContext _ctx;

        public OrderService(IApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<DTOs.OrderDto> CreateOrderAsync(string userId)
        {
            // Load cart items
            var cart = await _ctx.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Book)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("Cart is empty");

            // Build order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                OrderItems = cart.Items.Select(i => new OrderItem
                {
                    BookId = i.BookId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Book.Price
                }).ToList()
            };
            order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);

            _ctx.Orders.Add(order);

            // Mark books unavailable
            foreach (var item in order.OrderItems)
            {
                var book = await _ctx.Books.FindAsync(item.BookId);
                if (book != null) book.IsAvailable = false;
            }

            await _ctx.SaveChangesAsync();

            // Map to DTO
            return new OrderDto(
                order.Id,
                order.UserId,
                order.OrderDate,
                order.TotalAmount,
                order.OrderItems.Select(i => new OrderItemDto(
                    i.BookId, i.Quantity, i.UnitPrice
                )).ToList()
            );
        }

        public async Task<OrderDto> GetOrderAsync(int orderId)
        {
            var order = await _ctx.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return null;

            return new OrderDto(
                order.Id,
                order.UserId,
                order.OrderDate,
                order.TotalAmount,
                order.OrderItems.Select(i => new OrderItemDto(
                    i.BookId, i.Quantity, i.UnitPrice
                )).ToList()
            );
        }

        public async Task<List<OrderDto>> GetUserOrdersAsync(string userId)
        {
            return await _ctx.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Select(o => new OrderDto(
                    o.Id,
                    o.UserId,
                    o.OrderDate,
                    o.TotalAmount,
                    o.OrderItems.Select(i => new OrderItemDto(
                        i.BookId, i.Quantity, i.UnitPrice
                    )).ToList()
                ))
                .ToListAsync();
        }
    }
}
