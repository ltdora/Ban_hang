using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace He_thong_ban_hang
{
    public class ShopContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                  .HasKey(m => new { m.ProductID, m.OrderID});
        }
        public ShopContext(DbContextOptions options) : base(options) { }
        public DbSet<Employees> Employees { set; get; }
        public DbSet<Users> Uses { set; get; }
        public DbSet<Products> Products { set; get; }
        public DbSet<Order> Orders { set; get;}
        public DbSet<OrderDetail> OrderDetails { set; get; }
    }

}
