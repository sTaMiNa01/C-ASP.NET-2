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
            var r = userManager.CreateAsync(aUser, "Losenord123$$").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin.test@test.com";
            adminUser.Email = "admin.test@test.com";
            var adminUserResult = userManager.CreateAsync(adminUser, "Losenord123$$").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (!context.Dishes.Any())
            {
                var pizza = new Category() { CategoryID = 1, CategoryName = "Pizza" };
                var pasta = new Category() { CategoryID = 2, CategoryName = "Pasta" };
                var salad = new Category() { CategoryID = 3, CategoryName = "Salad" };

                var cheese = new Ingredient { IngredientID = 1, IngredientName = "Cheese" };
                var ham = new Ingredient { IngredientID = 2, IngredientName = "Ham" };
                var tomato = new Ingredient { IngredientID = 3, IngredientName = "Tomato" };
                var pineapple = new Ingredient { IngredientID = 4, IngredientName = "Pineapple" };
                var greenSalad = new Ingredient { IngredientID = 5, IngredientName = "Green Salad" };
                var pepper = new Ingredient { IngredientID = 6, IngredientName = "Pepper" };
                var olive = new Ingredient { IngredientID = 7, IngredientName = "Olive" };
                var cream = new Ingredient { IngredientID = 8, IngredientName = "Cream" };
                var pastaPenne = new Ingredient { IngredientID = 9, IngredientName = "Pasta penne" };
                var bolognese = new Ingredient { IngredientID = 10, IngredientName = "Bolognese" };

                var vesuvio = new Dish() { ID = 1, DishName = "Vesuvio", Price = 75, Category = pizza };
                var hawaii = new Dish() { ID = 2, DishName = "Hawaii", Price = 80, Category = pizza };
                var margaritha = new Dish() {ID = 3, DishName = "Margaritha", Price = 70, Category = pizza };
                var carbonara = new Dish() { ID = 4, DishName = "Carbonara", Price = 75, Category = pasta };
                var pastaBolognese = new Dish() { ID = 5, DishName = "Pasta Bolognese", Price = 80, Category = pasta };
                var veggiPasta = new Dish() { ID = 6, DishName = "Veggi Pasta", Price = 70, Category = pasta };

                var vesuvioCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };
                var vesuvioTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato };
                var vesuvioHam = new DishIngredient { Dish = margaritha, Ingredient = ham };

                var hawaiiTomato = new DishIngredient { Dish = hawaii, Ingredient = tomato };
                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };
                var hawaiiHam = new DishIngredient { Dish = hawaii, Ingredient = ham };

                var margarithaTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato };
                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };

                var carbonaraCheese = new DishIngredient { Dish = carbonara, Ingredient = cheese };
                var carrbonaraHam = new DishIngredient { Dish = carbonara, Ingredient = ham };
                var carbonaraCream = new DishIngredient { Dish = carbonara, Ingredient = cream };

                vesuvio.DishIngredients = new List<DishIngredient>();
                vesuvio.DishIngredients.Add(vesuvioCheese);
                vesuvio.DishIngredients.Add(vesuvioHam);
                vesuvio.DishIngredients.Add(vesuvioTomato);

                hawaii.DishIngredients = new List<DishIngredient>();
                hawaii.DishIngredients.Add(hawaiiTomato);
                hawaii.DishIngredients.Add(hawaiiCheese);
                hawaii.DishIngredients.Add(hawaiiPineapple);
                hawaii.DishIngredients.Add(hawaiiHam);

                margaritha.DishIngredients = new List<DishIngredient>();
                margaritha.DishIngredients.Add(margarithaTomato);
                margaritha.DishIngredients.Add(margarithaCheese);

                carbonara.DishIngredients = new List<DishIngredient>();
                carbonara.DishIngredients.Add(carbonaraCheese);
                carbonara.DishIngredients.Add(carbonaraCream);
                carbonara.DishIngredients.Add(carrbonaraHam);

                context.AddRange(vesuvio, hawaii, margaritha, carbonara, pastaBolognese, veggiPasta);
                context.AddRange(cheese, ham, tomato, pineapple, greenSalad, pastaPenne, bolognese, olive, pepper, cream);
                context.AddRange(pizza, pasta, salad);

                context.SaveChanges();
            }


        } 
    }
}
