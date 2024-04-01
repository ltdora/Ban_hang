using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace He_thong_ban_hang
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public ICollection<OrderDetail> orderDetails { get; }

    }
}
