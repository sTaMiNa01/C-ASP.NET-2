using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlinePizza.Data;
using OnlinePizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizza.Services
{
    public class UnitTestService
    {

        public int CartTotal(Cart cart)
        {
            int totalSum = 0;
            
            foreach (var dish in cart.CartItems)
            {
                totalSum += dish.Price;
            }

            return totalSum;
        }

        public int AddToCartTest(Cart cart, Dish dish)
        {
            int cartItemsSum;
            var newCartItemID = Guid.NewGuid();
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();
            CartItem cartItem = new CartItem();
            List<CartItem> cartItems = new List<CartItem>();

            foreach (var item in dish.DishIngredients)
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
            cartItem.Dish = dish;
            cartItem.Cart = cart;
            cartItem.CartID = cart.CartID;
            cartItem.CartItemIngredients = cartItemIngredient;
            cartItem.Price = dish.Price;

            cartItems.Add(cartItem);

            cart.CartItems = cartItems;

            cartItemsSum = cart.CartItems.Count();

            return cartItemsSum;
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
