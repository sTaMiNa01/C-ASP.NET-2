using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlinePizza.Data;
using Microsoft.AspNetCore.Identity;
using OnlinePizza.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlinePizza.ViewModels;

namespace OnlinePizza.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        List<CartItem> cartItems = new List<CartItem>();
        Cart cart = new Cart();

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Pay()
        {
            

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                return RedirectToAction("Index", "Dishes");
            }

            var cartID = HttpContext.Session.GetInt32("Cart");
            cart = await _context.Carts.Include(x => x.CartItems).SingleOrDefaultAsync(y => y.CartID == cartID);
            cartItems = cart.CartItems;

            var loggedInUser = _userManager.GetUserAsync(User).Result;

            int totalSum = OrderSum();

            var paymentItems = new PaymentViewModel()
            {
                Dishes = cartItems,
                OrderSum = totalSum
            };

            if (loggedInUser != null)
            {
                paymentItems.CustomerName = loggedInUser.Name;
                paymentItems.City = loggedInUser.City;
                paymentItems.Street = loggedInUser.Street;
                paymentItems.ZipCode = loggedInUser.Zipcode;
            }

            return View(paymentItems);
        }

        [HttpPost]
        public IActionResult Pay(PaymentViewModel paymentItems)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Receipt", paymentItems);
            }

            return View(paymentItems);
        }

        public async Task<IActionResult> Receipt()
        {
            var cartID = HttpContext.Session.GetInt32("Cart");
            cart = await _context.Carts.Include(x => x.CartItems).SingleOrDefaultAsync(y => y.CartID == cartID);
            cartItems = cart.CartItems;

            ViewData["OrderSum"] = OrderSum();

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return View();
        }

        public int OrderSum()
        {
            int totalSum = 0;
            foreach (var dish in cartItems)
            {
                totalSum += dish.Price;
            }

            return totalSum;
        }

    }
}
