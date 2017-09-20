using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class DishIngredient
    {
        public int DishID { get; set; }
        public Dish Dish { get; set; }
        public Guid IngredientID { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
