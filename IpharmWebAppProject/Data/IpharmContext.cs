using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IpharmWebAppProject.Models;

namespace IpharmWebAppProject.Data
{
    public class IpharmContext : DbContext
    {
        public IpharmContext(DbContextOptions<IpharmContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductInOrder> ProductInOrders { get; set; }
        public DbSet<ProductInWishList> ProductInWishLists { get; set; }

    }
}
