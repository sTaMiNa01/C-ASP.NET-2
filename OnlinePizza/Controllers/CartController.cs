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

        // GET: Cart/Details/5
        public async Task<ActionResult> AddToCart(int Id)
        {
            Dish dish = _context.Dishes.FirstOrDefault(p => p.ID == Id);

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                Cart cart = new Cart();
                List<CartItem> cartItems = new List<CartItem>();
                var carts = await _context.Carts.ToListAsync();
                int newID = carts.Count + 1;

                cartItems.Add(new CartItem { Dish = dish });

                cart.CartID = newID;
                cart.CartItems = cartItems;

                HttpContext.Session.SetInt32("Cart", newID);

                _context.Add(cart);
                await _context.SaveChangesAsync();

            } else
            {
                var cartID = HttpContext.Session.GetInt32("Cart");
                Cart cart = await _context.Carts.Include(x => x.CartItems).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartID == cartID);

                    var cartItem = await _context.CartItems.ToListAsync();
                    int newID = cartItem.Count + 1;

                    cart.CartItems.Add(new CartItem { CartItemID = newID, Dish = dish });

                _context.Update(cart);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Index", "Dishes");
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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

        // GET: Cart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cart/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}