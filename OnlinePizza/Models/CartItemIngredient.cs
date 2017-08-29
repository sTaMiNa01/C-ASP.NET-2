using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class CartItemIngredient
    {
        public int CartItemID { get; set; }
        public CartItem CartItem { get; set; }
        public int IngredientID { get; set; }
        public Ingredient Ingredient { get; set; }
        public bool Enabled { get; set; }
    }
}