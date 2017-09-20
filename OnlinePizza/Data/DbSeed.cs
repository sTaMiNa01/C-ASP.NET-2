using Microsoft.AspNetCore.Identity;
using OnlinePizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Data
{
    public static class DbSeed
    {

        public static void Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.Email = "student.test@test.com";
            aUser.UserName = "student.test@test.com";
            aUser.Street = "Vägen1";
            aUser.Zipcode = 12345;
            aUser.City = "Stockholm";
            var r = userManager.CreateAsync(aUser, "Losenord123$$").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin.test@test.com";
            adminUser.Email = "admin.test@test.com";
            adminUser.Street = "Vägen1";
            adminUser.Zipcode = 12345;
            adminUser.City = "Stockholm";
            var adminUserResult = userManager.CreateAsync(adminUser, "Losenord123$$").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (!context.Dishes.Any())
            {
                var pizza = new Category() { CategoryID = 1, CategoryName = "Pizza" };
                var pasta = new Category() { CategoryID = 2, CategoryName = "Pasta" };
                var salad = new Category() { CategoryID = 3, CategoryName = "Salad" };

#region         Ingredients
                var cheese = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Cheese", Price = 5 };
                var ham = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Ham", Price = 10 };
                var tomato = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Tomato", Price = 5 };
                var pineapple = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Pineapple", Price = 5 };
                var greenSalad = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Green Salad", Price = 5 };
                var pepper = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Pepper", Price = 5 };
                var olive = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Olive", Price = 5 };
                var cream = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Cream", Price = 5 };
                var pastaPenne = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Pasta penne", Price = 10 };
                var bolognese = new Ingredient { IngredientID = Guid.NewGuid(), IngredientName = "Bolognese", Price = 20 };
#endregion

                #region Dishes
                var vesuvio = new Dish() { ID = 1, DishName = "Vesuvio", Price = 75, CategoryID = pizza.CategoryID, Category = pizza };
                var hawaii = new Dish() { ID = 2, DishName = "Hawaii", Price = 80, CategoryID = pizza.CategoryID, Category = pizza };
                var margaritha = new Dish() {ID = 3, DishName = "Margaritha", Price = 70, CategoryID = pizza.CategoryID, Category = pizza };
                var carbonara = new Dish() { ID = 4, DishName = "Carbonara", Price = 75, CategoryID = pasta.CategoryID, Category = pasta };
                var pastaBolognese = new Dish() { ID = 5, DishName = "Pasta Bolognese", Price = 80, CategoryID = pasta.CategoryID, Category = pasta};
                var veggiPasta = new Dish() { ID = 6, DishName = "Veggi Pasta", Price = 70, CategoryID = pasta.CategoryID, Category = pasta};
                var veggiSalad = new Dish() { ID = 7, DishName = "Veggi Salad", Price = 60, CategoryID = salad.CategoryID, Category = salad };
                var cheeseSalad = new Dish() { ID = 8, DishName = "Cheese Salad", Price = 65, CategoryID = salad.CategoryID, Category = salad };
                var hamSalad = new Dish() { ID = 9, DishName = "Ham Salad", Price = 70, CategoryID = salad.CategoryID, Category = salad};
#endregion

                #region DishIngredients props
                var vesuvioCheese = new DishIngredient { Dish = vesuvio, Ingredient = cheese };
                var vesuvioTomato = new DishIngredient { Dish = vesuvio, Ingredient = tomato };
                var vesuvioHam = new DishIngredient { Dish = vesuvio, Ingredient = ham };

                var hawaiiTomato = new DishIngredient { Dish = hawaii, Ingredient = tomato };
                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };
                var hawaiiHam = new DishIngredient { Dish = hawaii, Ingredient = ham };

                var margarithaTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato };
                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };

                var pastaBolognesePasta = new DishIngredient { Dish = pastaBolognese, Ingredient = pastaPenne };
                var pastaBologneseMeat = new DishIngredient { Dish = pastaBolognese, Ingredient = bolognese };

                var carbonaraCheese = new DishIngredient { Dish = carbonara, DishID = carbonara.ID, Ingredient = cheese, IngredientID = cheese.IngredientID };
                var carrbonaraHam = new DishIngredient { Dish = carbonara, DishID = carbonara.ID, Ingredient = ham, IngredientID = ham.IngredientID };
                var carbonaraCream = new DishIngredient { Dish = carbonara, DishID = carbonara.ID, Ingredient = cream, IngredientID = cream.IngredientID };
                var carbonaraPasta = new DishIngredient { Dish = carbonara, DishID = carbonara.ID, Ingredient = pastaPenne, IngredientID = pastaPenne.IngredientID };

                var veggiPastapasta = new DishIngredient { Dish = veggiPasta, Ingredient = pastaPenne };
                var veggiPastaCream = new DishIngredient { Dish = veggiPasta, Ingredient = cream };
                var veggiPastaPepper = new DishIngredient { Dish = veggiPasta, Ingredient = pepper };

                var veggiSaladgreen = new DishIngredient { Dish = veggiSalad, Ingredient = greenSalad };
                var veggiSaladOlive = new DishIngredient { Dish = veggiSalad, Ingredient = olive };
                var veggiSaladPepper = new DishIngredient { Dish = veggiSalad, Ingredient = pepper };

                var cheeseSaladgren = new DishIngredient { Dish = cheeseSalad, Ingredient = greenSalad };
                var cheeseSaladCheese = new DishIngredient { Dish = cheeseSalad, Ingredient = cheese };
                var cheeseSaladPepper = new DishIngredient { Dish = cheeseSalad, Ingredient = pepper };

                var hamSaladgreen = new DishIngredient { Dish = hamSalad, Ingredient = greenSalad };
                var hamSaladHam = new DishIngredient { Dish = hamSalad, Ingredient = ham };
                var hamSaladPepper = new DishIngredient { Dish = hamSalad, Ingredient = pepper };
#endregion

                #region DishIngredients Lists
                vesuvio.DishIngredients = new List<DishIngredient>();
                vesuvio.DishIngredients.Add(vesuvioCheese);
                vesuvio.DishIngredients.Add(vesuvioTomato);
                vesuvio.DishIngredients.Add(vesuvioHam);

                hawaii.DishIngredients = new List<DishIngredient>();
                hawaii.DishIngredients.Add(hawaiiCheese);
                hawaii.DishIngredients.Add(hawaiiHam);
                hawaii.DishIngredients.Add(hawaiiPineapple);
                hawaii.DishIngredients.Add(hawaiiTomato);

                margaritha.DishIngredients = new List<DishIngredient>();
                margaritha.DishIngredients.Add(margarithaCheese);
                margaritha.DishIngredients.Add(margarithaTomato);

                carbonara.DishIngredients = new List<DishIngredient>();
                carbonara.DishIngredients.Add(carbonaraCheese);
                carbonara.DishIngredients.Add(carbonaraCream);
                carbonara.DishIngredients.Add(carbonaraPasta);
                carbonara.DishIngredients.Add(carrbonaraHam);

                pastaBolognese.DishIngredients = new List<DishIngredient>();
                pastaBolognese.DishIngredients.Add(pastaBologneseMeat);
                pastaBolognese.DishIngredients.Add(pastaBolognesePasta);

                veggiPasta.DishIngredients = new List<DishIngredient>();
                veggiPasta.DishIngredients.Add(veggiPastapasta);
                veggiPasta.DishIngredients.Add(veggiPastaCream);
                veggiPasta.DishIngredients.Add(veggiPastaPepper);

                veggiSalad.DishIngredients = new List<DishIngredient>();
                veggiSalad.DishIngredients.Add(veggiSaladgreen);
                veggiSalad.DishIngredients.Add(veggiSaladOlive);
                veggiSalad.DishIngredients.Add(veggiSaladPepper);

                cheeseSalad.DishIngredients = new List<DishIngredient>();
                cheeseSalad.DishIngredients.Add(cheeseSaladCheese);
                cheeseSalad.DishIngredients.Add(cheeseSaladCheese);
                cheeseSalad.DishIngredients.Add(cheeseSaladPepper);

                hamSalad.DishIngredients = new List<DishIngredient>();
                hamSalad.DishIngredients.Add(hamSaladgreen);
                hamSalad.DishIngredients.Add(hamSaladHam);
                hamSalad.DishIngredients.Add(hamSaladPepper);
#endregion

                context.AddRange(cheese, ham, tomato, pineapple, greenSalad, pastaPenne, bolognese, olive, pepper, cream);
                context.AddRange(pizza, pasta, salad);
                context.AddRange(vesuvio, hawaii, margaritha, carbonara, pastaBolognese, veggiPasta, veggiSalad, cheeseSalad, hamSalad);

                context.SaveChanges();
            }
        } 
    }
}
