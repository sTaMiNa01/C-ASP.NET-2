using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePizza.Data;
using OnlinePizza.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlinePizza.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<ActionResult> Index()
        {
            List<CartItem> cartItems = new List<CartItem>();

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                var carts = await _context.Carts.ToListAsync();
                var newID = HttpContext.Session.GetInt32("Cart");
                newID = carts.Count + 1;
                cartItems = new List<CartItem>();
            }
            else
            {
                Cart cart = new Cart();

                var cartID = HttpContext.Session.GetInt32("Cart");

                cart = await _context.Carts.Include(x => x.CartItems).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartID == cartID);
                cartItems = cart.CartItems;

            }
            return View(cartItems);
        }

        // GET: Cart/AddToCart/5
        public async Task<ActionResult> AddToCart(int Id)
        {
            //Dish dish = _context.Dishes.FirstOrDefault(p => p.ID == Id);
            Dish dish = await _context.Dishes.Include(x => x.DishIngredients).ThenInclude(i => i.Ingredient).SingleOrDefaultAsync(d => d.ID == Id);
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();
            CartItem cartItem = new CartItem();

            if (HttpContext.Session.GetInt32("Cart") == null)
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
                        CartItemIngredientPrice = item.Ingredient.Price
                    };

                    cartItemIngredient.Add(newCartItemIngredient);
                }

                cartItems.Add(new CartItem { Dish = dish, CartID = newID, Cart = cart, CartItemIngredients = cartItemIngredient, CartItemID = newCartItemID });

                cart.CartID = newID;
                cart.CartItems = cartItems;

                HttpContext.Session.SetInt32("Cart", newID);

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

            } else
            {
                var cartID = HttpContext.Session.GetInt32("Cart");
                Cart cart = await _context.Carts.Include(x => x.CartItems).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartID == cartID);

                var newCartItemID = Guid.NewGuid();

                foreach (var item in dish.DishIngredients)
                {
                    var newCartItemIngredient = new CartItemIngredient
                    {
                        CartItem = cartItem,
                        CartItemID = newCartItemID,
                        IngredientName = item.Ingredient.IngredientName,
                        CartItemIngredientPrice = item.Ingredient.Price
                    };

                    cartItemIngredient.Add(newCartItemIngredient);
                }

                cart.CartItems.Add(new CartItem { CartItemID = newCartItemID, Dish = dish, Cart = cart, CartID = cart.CartID, CartItemIngredients = cartItemIngredient });

                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Index", "Dishes");
        }


        // GET: Cart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cart/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cart/RemoveFromCart/5
        public async Task<ActionResult> RemoveFromCart(Guid id)
        {
            var cartID = HttpContext.Session.GetInt32("Cart");
            Cart cart = await _context.Carts.Include(x => x.CartItems).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartID == cartID);
            var cartItem = _context.CartItems.First(ci => ci.CartItemID == id);

            if (cartItem != null)
            {
                //cartItem.CartItemIngredients.RemoveAll(ci => ci.CartItemID == cartItem.CartItemID);
                cart.CartItems.RemoveAll(ci => ci.CartItemID == cartItem.CartItemID);
                
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Dishes");
        }
    }
}