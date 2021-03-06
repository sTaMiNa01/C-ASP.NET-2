﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Models
{
    public class Dish
    {
        public int ID { get; set; }
        public string DishName { get; set; }
        public int Price { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
