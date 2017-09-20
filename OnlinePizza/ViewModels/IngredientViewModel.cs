using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.ViewModels
{
    public class IngredientViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool Selected { get; set; }

    }
}
