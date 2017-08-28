using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public Dish Dish { get; set; }
        public int CartID { get; set; }
        public int Quantity { get; set; }
        public List<Ingredient> ExtraIngredient { get; set; }
    }
}
