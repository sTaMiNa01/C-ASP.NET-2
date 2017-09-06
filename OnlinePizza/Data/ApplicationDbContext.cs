using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlinePizza.Models;

namespace OnlinePizza.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishID, di.IngredientID});


                builder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishID);

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.IngredientID);


            builder.Entity<Dish>()
                .HasOne(d => d.Category)
                .WithMany(d => d.Dishes)
                .HasForeignKey(d => d.CategoryID);

            builder.Entity<Cart>()
             .HasKey(cart => cart.CartID);

            builder.Entity<CartItem>()
            .HasOne(di => di.Cart)
             .WithMany(d => d.CartItems)
            .HasForeignKey(di => di.CartID);

            builder.Entity<CartItemIngredient>()
                .HasOne(di => di.CartItem)
                .WithMany(d => d.CartItemIngredients)
                .HasForeignKey(di => di.CartItemID);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemIngredient> CartItemIngredients { get; set; }
    }
}
