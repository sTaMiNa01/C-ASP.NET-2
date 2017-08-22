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

namespace OnlinePizza.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Price")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
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
                Id = x.IngredientID,
                Name = x.IngredientName,
                Selected = dish.DishIngredients.Any(k => k.IngredientID.Equals(x.IngredientID) ? true : false)
            }).ToListAsync();

            var viewModel = new DishViewModel()
            {
                DishId = dish.ID,
                Name = dish.Name,
                Price = dish.Price,
                CategoryId = dish.Category.CategoryId,
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
        //[Bind("DishId,Name,Price,Ingredients")] Dish dish
        public async Task<IActionResult> Edit(DishViewModel model)
        {

            if (ModelState.IsValid)
            {
                var dish = _context.Dishes.Include(x => x.DishIngredients).FirstOrDefault(x => x.ID.Equals(model.DishId));
                dish.CategoryId = model.CategoryId;
                dish.Name = model.Name;
                dish.Price = model.Price;

                foreach (var ingredient in model.Ingredients)
                {
                    if (ingredient.Selected && !dish.DishIngredients.Any(x => x.IngredientID.Equals(ingredient.Id)))
                    {
                        dish.DishIngredients.Add(new DishIngredient() { IngredientID = ingredient.Id });
                    }
                    else if (!ingredient.Selected && dish.DishIngredients.Any(x => x.IngredientID.Equals(ingredient.Id)))
                    {
                        dish.DishIngredients.RemoveAll(x => x.IngredientID.Equals(ingredient.Id));
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
