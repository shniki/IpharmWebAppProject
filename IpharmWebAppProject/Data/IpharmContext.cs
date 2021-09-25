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

        protected override void OnModelCreating(ModelBuilder modelBuilder) //Create connections (M2M)
        {
            modelBuilder.Entity<ProductInOrder>().HasKey(po => new { po.ProductId, po.OrderId});
            modelBuilder.Entity<ProductInOrder>().HasOne(po => po.Product).WithMany(o => o.InOrders).HasForeignKey(po => po.ProductId);
            modelBuilder.Entity<ProductInOrder>().HasOne(po => po.Order).WithMany(o => o.Products).HasForeignKey(po => po.OrderId);
            modelBuilder.Entity<ProductInWishList>().HasKey(po => new { po.ProductId, po.WishListId });
            modelBuilder.Entity<ProductInWishList>().HasOne(po => po.Product).WithMany(o => o.InWishList).HasForeignKey(po => po.ProductId);
            modelBuilder.Entity<ProductInWishList>().HasOne(po => po.WishList).WithMany(o => o.Products).HasForeignKey(po => po.WishListId);

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
