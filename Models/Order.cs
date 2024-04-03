using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace He_thong_ban_hang
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public int Status { get; set; }

        public DateTime CreatedTime { get; set; }

        public decimal Total { set; get; }

        public ICollection<OrderDetail> orderDetail { get; }
    }
}
