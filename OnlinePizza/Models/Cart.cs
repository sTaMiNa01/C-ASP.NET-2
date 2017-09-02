using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public ApplicationUser Customer { get; set; }
        public int CustomerID { get; set; }
        public List<CartItem> CartItems { get; set; }
        public int TotalAmount { get; set; }
    }
}
