using OnlinePizza.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class CustomCartDish
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public List<IngredientViewModel> Ingredients { get; set; }
        public string CategoryName { get; set; }

    }
}
