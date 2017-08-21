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
                var cheese = new Ingredient { IngredientID = 1, IngredientName = "Cheese" };
                var ham = new Ingredient { IngredientID = 2, IngredientName = "Ham" };
                var tomato = new Ingredient { IngredientID = 3, IngredientName = "Tomato" };
                var pineapple = new Ingredient { IngredientID = 4, IngredientName = "Pineapple" };

                var vesuvio = new Dish() { ID = 1, Name = "Vesuvio", Price = 75 };
                var hawaii = new Dish() { ID = 2, Name = "Hawaii", Price = 80 };
                var margaritha = new Dish() {ID = 3, Name = "Margaritha", Price = 70 };

                var vesuvioCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };
                var vesuvioTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato };
                var vesuvioHam = new DishIngredient { Dish = margaritha, Ingredient = ham };

                var hawaiiTomato = new DishIngredient { Dish = hawaii, Ingredient = tomato };
                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };
                var hawaiiHam = new DishIngredient { Dish = hawaii, Ingredient = ham };

                var margarithaTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato };
                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };

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

                context.AddRange(vesuvio, hawaii, margaritha);
                context.AddRange(cheese, ham, tomato, pineapple);
                context.SaveChanges();
            }


        } 
    }
}
