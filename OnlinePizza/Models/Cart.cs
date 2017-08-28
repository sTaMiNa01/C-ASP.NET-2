using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public string Customer { get; set; }
        public List<CartItem> CartItems { get; set; }
        public bool Delivered { get; set; }
    }
}
