using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class CartItemIngredient
    {
        //public int CartItemID { get; set; }
        //public CartItem CartItem { get; set; }
        //public int IngredientID { get; set; }
        //public Ingredient Ingredient { get; set; }
        //public bool Enabled { get; set; }

        public int CartItemIngredientID { get; set; }
        public Guid CartItemID { get; set; }
        public CartItem CartItem { get; set; }
        [DisplayName("Ingredient")]
        public string IngredientName { get; set; }
        public int CartItemIngredientPrice { get; set; }
    }
}