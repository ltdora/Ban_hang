using System.ComponentModel.DataAnnotations;

namespace He_thong_ban_hang
{
    public class ShoppingCart
    {
        [Key]
        public int CartID { get; set; }
        
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public Products Products { get; set; }
    }
}
