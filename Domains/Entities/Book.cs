using Infastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
