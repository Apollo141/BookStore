using Domain.Entities;
using Infastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Book> Books { get; }
        DbSet<ShoppingCart> ShoppingCarts { get; }
        DbSet<ShoppingCartItem> ShoppingCartItems { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
