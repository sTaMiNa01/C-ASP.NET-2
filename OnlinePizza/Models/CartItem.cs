using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class CartItem
    {
        public int CartItemID { get; set; }
        public Cart Cart { get; set; }
        public Dish Dish { get; set; }
        public int DishID { get; set; }
        public int CartID { get; set; }
        public List<CartItemIngredient> CartItemIngredients { get; set; }
    }
}
