using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Menu
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            FoodItems = await _context.FoodItems.ToListAsync();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int foodItemId, int quantity = 1)
        {
            // Check if user is logged in
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return new JsonResult(new { 
                    success = false, 
                    message = "Please login to add items to cart",
                    requiresLogin = true 
                });
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return new JsonResult(new { 
                    success = false, 
                    message = "Invalid user session. Please login again.",
                    requiresLogin = true 
                });
            }

            var foodItem = await _context.FoodItems.FindAsync(foodItemId);
            if (foodItem == null)
            {
                return new JsonResult(new { 
                    success = false, 
                    message = "Food item not found" 
                });
            }

            // Check if item already exists in user's cart
            var existingCartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.FoodItemId == foodItemId);

            if (existingCartItem != null)
            {
                // Update existing item quantity
                existingCartItem.Quantity += quantity;
                _context.Update(existingCartItem);
            }
            else
            {
                // Create new cart item
                var newCartItem = new CartItem
                {
                    UserId = userId,
                    FoodItemId = foodItem.Id,
                    FoodName = foodItem.Name,
                    Price = foodItem.Price,
                    Quantity = quantity,
                    CreatedAt = DateTime.Now
                };
                
                _context.CartItems.Add(newCartItem);
            }

            await _context.SaveChangesAsync();

            return new JsonResult(new { 
                success = true, 
                message = $"{foodItem.Name} added to cart!" 
            });
        }
    }
}
