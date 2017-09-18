using OnlinePizza.Models;
using OnlinePizza.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace OnlinePizzaTest
{
    public class CartTest
    {
        private readonly UnitTestService _UnitTestService;


        public CartTest()
        {
            _UnitTestService = new UnitTestService();
        }

        [Fact]
        public void CartSumTest()
        {
#region     MockUpData
            var pizza = new Category() { CategoryID = 1, CategoryName = "Pizza" };

            var cheese = new Ingredient { IngredientID = 1, IngredientName = "Cheese", Price = 5 };
            var ham = new Ingredient { IngredientID = 2, IngredientName = "Ham", Price = 10 };
            var tomato = new Ingredient { IngredientID = 3, IngredientName = "Tomato", Price = 5 };

            var vesuvio = new Dish() { ID = 1, DishName = "Vesuvio", Price = 75, CategoryID = pizza.CategoryID, Category = pizza };

            var vesuvioCheese = new DishIngredient { Dish = vesuvio, Ingredient = cheese };
            var vesuvioTomato = new DishIngredient { Dish = vesuvio, Ingredient = tomato };
            var vesuvioHam = new DishIngredient { Dish = vesuvio, Ingredient = ham };

            vesuvio.DishIngredients = new List<DishIngredient>();
            vesuvio.DishIngredients.Add(vesuvioCheese);
            vesuvio.DishIngredients.Add(vesuvioTomato);
            vesuvio.DishIngredients.Add(vesuvioHam);

            #endregion

            Cart cart = new Cart();
            cart.CartID = 1;

            List<CartItem> cartItems = new List<CartItem>();
            var newCartItemID = Guid.NewGuid();
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();
            CartItem cartItem = new CartItem();

            foreach (var item in vesuvio.DishIngredients)
            {
                var newCartItemIngredient = new CartItemIngredient
                {
                    CartItem = cartItem,
                    CartItemID = newCartItemID,
                    IngredientName = item.Ingredient.IngredientName,
                    CartItemIngredientPrice = item.Ingredient.Price,
                    Selected = true,
                    CartItemIngredientID = GenerateCartItemIngredientID()
                };

                cartItemIngredient.Add(newCartItemIngredient);
            }

            cartItem.CartItemID = newCartItemID;
            cartItem.Dish = vesuvio;
            cartItem.Cart = cart;
            cartItem.CartID = cart.CartID;
            cartItem.CartItemIngredients = cartItemIngredient;
            cartItem.Price = vesuvio.Price;

            cartItems.Add(cartItem);

            cart.CartItems = cartItems;

            var actual = vesuvio.Price;

            var result = _UnitTestService.CartTotal(cart);

            Assert.Equal(actual,result);

        }

        [Fact]
        public void AddToCartTest()
        {
#region MockUpData
            var pasta = new Category() { CategoryID = 2, CategoryName = "Pasta" };

            var cream = new Ingredient { IngredientID = 8, IngredientName = "Cream", Price = 5 };
            var pastaPenne = new Ingredient { IngredientID = 9, IngredientName = "Pasta penne", Price = 10 };
            var pepper = new Ingredient { IngredientID = 6, IngredientName = "Pepper", Price = 5 };

            var veggiPasta = new Dish() { ID = 6, DishName = "Veggi Pasta", Price = 70, CategoryID = pasta.CategoryID, Category = pasta };

            var veggiPastapasta = new DishIngredient { Dish = veggiPasta, Ingredient = pastaPenne };
            var veggiPastaCream = new DishIngredient { Dish = veggiPasta, Ingredient = cream };
            var veggiPastaPepper = new DishIngredient { Dish = veggiPasta, Ingredient = pepper };

            veggiPasta.DishIngredients = new List<DishIngredient>();
            veggiPasta.DishIngredients.Add(veggiPastapasta);
            veggiPasta.DishIngredients.Add(veggiPastaCream);
            veggiPasta.DishIngredients.Add(veggiPastaPepper);
            #endregion

            Cart cart = new Cart();
            cart.CartID = 2;

            var actual = 1;

            var result = _UnitTestService.AddToCartTest(cart, veggiPasta);

            Assert.Equal(actual, result);

        }

        public int GenerateCartItemIngredientID()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
