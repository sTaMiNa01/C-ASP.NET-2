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
    public class CartService
    {
        List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();
        CartItem cartItem = new CartItem();
        ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<Cart> AddToNewCart(Dish dish)
        {
            Cart cart = new Cart();
            List<CartItem> cartItems = new List<CartItem>();
            var carts = await _context.Carts.ToListAsync();
            int newID = carts.Count + 1;
            var newCartItemID = Guid.NewGuid();

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

            cartItems.Add(new CartItem
            {
                Dish = dish,
                CartID = newID,
                Cart = cart,
                CartItemIngredients = cartItemIngredient,
                CartItemID = newCartItemID,
                Price = dish.Price
            });

            cart.CartID = newID;
            cart.CartItems = cartItems;

           await _context.Carts.AddAsync(cart);
           await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> AddToExistingCart(Dish dish, Cart cart)
        {
            var newCartItemID = Guid.NewGuid();

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

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return (cart);
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            List<CartItem> cartItems = new List<CartItem>();

            if (_session.GetInt32("Cart") == null)
            {
                var carts = await _context.Carts.ToListAsync();
                var newID = _session.GetInt32("Cart");
                newID = carts.Count + 1;
                cartItems = new List<CartItem>();
            }
            else
            {
                Cart cart = new Cart();

                var cartID = _session.GetInt32("Cart");

                cart = await _context.Carts
                    .Include(x => x.CartItems)
                    .ThenInclude(z => z.Dish)
                    .SingleOrDefaultAsync(y => y.CartID == cartID);

                cartItems = cart.CartItems;

            }

            return (cartItems);
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
