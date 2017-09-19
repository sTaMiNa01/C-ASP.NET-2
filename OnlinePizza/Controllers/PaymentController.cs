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
using OnlinePizza.Services;

namespace OnlinePizza.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly CartService _cartService;

        List<CartItem> cartItems = new List<CartItem>();
        Cart cart = new Cart();

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, CartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Pay()
        {
           
            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                return RedirectToAction("Index", "Dishes");
            }

            cart = await _cartService.GetCart();

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

            ViewData["OrderSum"] = OrderSum();

            return View(paymentItems);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(PaymentViewModel paymentItems)
        {

            if (ModelState.IsValid)
            {
                return RedirectToAction("Receipt", paymentItems);
            }

            cart = await _cartService.GetCart();

            cartItems = cart.CartItems;

            ViewData["OrderSum"] = OrderSum();

            return View(paymentItems);
        }

        public async Task<IActionResult> Receipt()
        {
            cart = await _cartService.GetCart();

            cartItems = cart.CartItems;

            ViewData["OrderSum"] = OrderSum();

            HttpContext.Session.Clear();
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
