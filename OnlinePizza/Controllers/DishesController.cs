using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlinePizza.Data;
using OnlinePizza.Models;
using OnlinePizza.ViewModels;
using Microsoft.AspNetCore.Authorization;
using OnlinePizza.Services;

namespace OnlinePizza.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public DishesController(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            List<CartItem> cartItems = await _cartService.GetCartItems();
            ViewData["CartItemsList"] = cartItems;

            return View(await _context.Dishes.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var allCategories = await _context.Categories.ToListAsync();
            var allIngredients = await _context.Ingredients.Select(x => new IngredientViewModel()
            {
                ID = x.IngredientID,
                Name = x.IngredientName

            }).ToListAsync();

            var viewModel = new DishViewModel() { Ingredients = allIngredients, Categories = allCategories };

            return View(viewModel);
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(DishViewModel model)
        {
            Dish newDish = new Dish();
            List<DishIngredient> dishIngrediends = new List<DishIngredient>();

            if (ModelState.IsValid)
            {
                var dishes = await _context.Dishes.ToListAsync();
                int newID = dishes.Count + 1;

                foreach (var item in model.Ingredients)
                {
                    DishIngredient newDishes = new DishIngredient();
                    if (item.Selected == true)
                    {
                        newDishes.IngredientID = item.ID;
                        newDishes.DishID = newID;
                        dishIngrediends.Add(newDishes);

                    }

                }

                newDish.DishName = model.Name;
                newDish.ID = newID;
                newDish.DishIngredients = dishIngrediends;
                newDish.Price = model.Price;
                newDish.CategoryID = model.CategoryID;

                _context.Add(newDish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            return View(newDish);
        }

        // GET: Dishes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.
                Include(x => x.Category).
                Include(x => x.DishIngredients).
                ThenInclude(x => x.Ingredient).
                SingleOrDefaultAsync(m => m.ID == id);

            var allCategories = await _context.Categories.ToListAsync();
            var allIngredients = await _context.Ingredients.Select(x => new IngredientViewModel()
            {
                ID = x.IngredientID,
                Name = x.IngredientName,
                Selected = dish.DishIngredients.Any(k => k.IngredientID.Equals(x.IngredientID) ? true : false)

            }).ToListAsync();

            var viewModel = new DishViewModel()
            {
                DishID = dish.ID,
                Name = dish.DishName,
                Price = dish.Price,
                CategoryID = dish.Category.CategoryID,
                Ingredients = allIngredients,
                Categories = allCategories
            };

            if (dish == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(DishViewModel model)
        {

            if (ModelState.IsValid)
            {
                var dish = _context.Dishes.Include(x => x.DishIngredients).FirstOrDefault(x => x.ID.Equals(model.DishID));
                dish.CategoryID = model.CategoryID;
                dish.DishName = model.Name;
                dish.Price = model.Price;

                foreach (var ingredient in model.Ingredients)
                {
                    if (ingredient.Selected && !dish.DishIngredients.Any(x => x.IngredientID.Equals(ingredient.ID)))
                    {
                        dish.DishIngredients.Add(new DishIngredient() { IngredientID = ingredient.ID });
                    }
                    else if (!ingredient.Selected && dish.DishIngredients.Any(x => x.IngredientID.Equals(ingredient.ID)))
                    {
                        dish.DishIngredients.RemoveAll(x => x.IngredientID.Equals(ingredient.ID));
                    }
                }

                try
                {
                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Dishes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.ID == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.ID == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.ID == id);
        }
    }
}
