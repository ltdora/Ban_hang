using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions options) : base(options) { }
        DbSet<Employees> Employees { set; get; }
        DbSet<Users> uses { set; get; }
    }
}
