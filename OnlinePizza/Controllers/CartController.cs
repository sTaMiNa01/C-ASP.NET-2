using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePizza.Data;
using OnlinePizza.Models;
using Microsoft.EntityFrameworkCore;
using OnlinePizza.ViewModels;
using OnlinePizza.Services;

namespace OnlinePizza.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public CartController(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

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

                cart = await _context.Carts
                    .Include(x => x.CartItems)
                    .ThenInclude(z => z.Dish)
                    .SingleOrDefaultAsync(y => y.CartID == cartID);

                cartItems = cart.CartItems;

                ViewData["CartItemsList"] = cartItems;
            }
            return View(cartItems);
        }

        public async Task<ActionResult> AddToCart(int? Id)
        {
            var dish = await _context.Dishes
                .Include(x => x.Category)
                .Include(x => x.DishIngredients)
                .ThenInclude(x => x.Ingredient)
                .SingleOrDefaultAsync(m => m.ID == Id);

            int cartsID;

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                var newCart = await _cartService.AddToNewCart(dish);
                cartsID = newCart.CartID;

                HttpContext.Session.SetInt32("Cart", cartsID);


            } else
            {
                var cartID = HttpContext.Session.GetInt32("Cart");
                Cart cart = await _context.Carts
                    .Include(x => x.CartItems)
                    .ThenInclude(z => z.Dish)
                    .SingleOrDefaultAsync(y => y.CartID == cartID);

                var addToCart = await _cartService.AddToExistingCart(dish, cart);

            }

            return RedirectToAction("Index", "Dishes");
        }

        public async Task<ActionResult> EditCartItem(Guid id)
        {
            CartItem cartItem = _context.CartItems
                .Include(x => x.CartItemIngredients)
                .Include(z => z.Dish).SingleOrDefault(y => y.CartItemID == id);

            var extra = _context.Ingredients
                .Where(x => !cartItem.CartItemIngredients.Any(s => s.IngredientName.Equals(x.IngredientName))).ToList();

            var extraIngredients = extra.Select(x => new CartItemIngredient()
            {
                CartItemIngredientID = _cartService.GenerateCartItemIngredientID(),
                CartItemID = Guid.NewGuid(),
                IngredientName = x.IngredientName,
                Selected = false,
                CartItemIngredientPrice = x.Price

            }).ToList();

            cartItem.ExtraCartItemIngredients = extraIngredients;

            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            return View(cartItem);
        }

        [HttpPost]
        public async Task<ActionResult> EditCartItem(CartItem model)
        {
            CartItem cartItem = _context.CartItems
                .Include(x => x.CartItemIngredients)
                .Include(i => i.ExtraCartItemIngredients)
                .Include(z => z.Dish).SingleOrDefault(y => y.CartItemID == model.CartItemID);

            List<CartItemIngredient> clearExtraIngredients = new List<CartItemIngredient>();

            if(model.ExtraCartItemIngredients == null)
            {

                return RedirectToAction("Index");

            } else
            {
                foreach (var ingredient in model.ExtraCartItemIngredients)
                {
                    if (ingredient.Selected && !cartItem.CartItemIngredients
                        .Any(x => x.CartItemIngredientID.Equals(ingredient.CartItemIngredientID)))
                    {
                        CartItemIngredient newIngredient = _context.CartItemIngredients
                            .SingleOrDefault(s => s.CartItemIngredientID == ingredient.CartItemIngredientID);

                        var newCartItemIngredient = new CartItemIngredient
                        {
                            CartItem = cartItem,
                            CartItemID = cartItem.CartItemID,
                            IngredientName = newIngredient.IngredientName,
                            CartItemIngredientPrice = newIngredient.CartItemIngredientPrice,
                            Selected = true,
                            CartItemIngredientID = _cartService.GenerateCartItemIngredientID()
                        };

                        cartItem.CartItemIngredients.Add(newCartItemIngredient);
                        cartItem.Price = cartItem.Price + newIngredient.CartItemIngredientPrice;
                        cartItem.ExtraCartItemIngredients = clearExtraIngredients;
                    }
                }
            }

            if (model.CartItemIngredients == null)
            {

                return RedirectToAction("Index");

            }
            else
            {
                foreach (var orginalIngredient in model.CartItemIngredients)
                {
                    CartItemIngredient ingredientToRemove = _context.CartItemIngredients
                        .SingleOrDefault(s => s.CartItemIngredientID == orginalIngredient.CartItemIngredientID);

                    if (!orginalIngredient.Selected && cartItem.CartItemIngredients
                        .Any(x => x.CartItemIngredientID.Equals(orginalIngredient.CartItemIngredientID)))
                    {
                        cartItem.CartItemIngredients.RemoveAll(x => x.CartItemIngredientID.Equals(orginalIngredient.CartItemIngredientID));
                        cartItem.Price = cartItem.Price - ingredientToRemove.CartItemIngredientPrice;
                        cartItem.ExtraCartItemIngredients = clearExtraIngredients;
                    }

                }
            }

            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RemoveFromCart(Guid id)
        {
            var cartID = HttpContext.Session.GetInt32("Cart");

            Cart cart = await _context.Carts
                .Include(x => x.CartItems).ThenInclude(z => z.Dish)
                .SingleOrDefaultAsync(y => y.CartID == cartID);

            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartItemID == id);

            if (cartItem != null)
            {
                cart.CartItems.RemoveAll(ci => ci.CartItemID == cartItem.CartItemID);
                
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ClearCart()
        {
            var cartID = HttpContext.Session.GetInt32("Cart");

            Cart cart = await _context.Carts
                .Include(x => x.CartItems)
                .ThenInclude(z => z.Dish)
                .SingleOrDefaultAsync(y => y.CartID == cartID);

            HttpContext.Session.Clear();
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Dishes");
        }


    }
}