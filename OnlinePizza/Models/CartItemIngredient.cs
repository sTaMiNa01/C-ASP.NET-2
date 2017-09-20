using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class CartItemIngredient
    {
        public Guid CartItemIngredientID { get; set; }
        public Guid CartItemID { get; set; }
        public CartItem CartItem { get; set; }
        [DisplayName("Ingredient")]
        public string IngredientName { get; set; }
        public int CartItemIngredientPrice { get; set; }
        public bool Selected { get; set; }

    }
}