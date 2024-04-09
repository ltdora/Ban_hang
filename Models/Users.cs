using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class Users
    {
        [Key]
        public int UserId { set; get; }
        public string UserName { set; get; }
        public string UserPassword { set; get; }
        public List<Order> order { set; get; }
    }
}
