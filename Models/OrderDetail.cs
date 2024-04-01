using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class OrderDetail
    {
        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
