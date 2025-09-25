using FoodOrderingSystem.Helpers;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartItem> Cart { get; set; } = new List<CartItem>();
        public decimal Total => Cart?.Sum(i => i.Price * i.Quantity) ?? 0;
        public bool IsLoggedIn => !string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"));

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                // User not logged in - redirect to login page
                return RedirectToPage("/Account/Login", new { returnUrl = "/Cart" });
            }

            if (int.TryParse(userIdString, out int userId))
            {
                // Load cart items from database for the logged-in user
                Cart = await _context.CartItems
                    .Include(c => c.FoodItem)
                    .Where(c => c.UserId == userId)
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(int itemId, int quantity)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new JsonResult(new { success = false, message = "Please login to manage cart" });
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == itemId && c.UserId == userId);
            
            if (cartItem != null)
            {
                if (quantity <= 0)
                {
                    _context.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                    _context.Update(cartItem);
                }
                
                await _context.SaveChangesAsync();
                return new JsonResult(new { success = true });
            }
            
            return new JsonResult(new { success = false, message = "Item not found" });
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int itemId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new JsonResult(new { success = false, message = "Please login to manage cart" });
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == itemId && c.UserId == userId);
                
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostClearCartAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new JsonResult(new { success = false, message = "Please login to manage cart" });
            }

            var userCartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();
                
            _context.CartItems.RemoveRange(userCartItems);
            await _context.SaveChangesAsync();
            
            return new JsonResult(new { success = true });
        }
    }
}
