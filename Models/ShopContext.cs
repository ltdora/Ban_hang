using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class ShopContext : DbContext
    {
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<OrderDetail>(
        //            eb =>
        //            {
        //                eb.HasNoKey();
        //                //eb.ToView("View_BlogPostCounts");
        //                //eb.Property(v => v.BlogName).HasColumnName("Name");
        //            });
        //}
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
