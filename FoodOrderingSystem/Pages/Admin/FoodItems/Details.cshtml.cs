using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Admin.FoodItems
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public FoodItem FoodItem { get; set; } = new FoodItem();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check if user is logged in as admin
            if (!AuthHelper.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { returnUrl = $"/Admin/FoodItems/Details?id={id}" });
            }

            if (id == null)
            {
                return NotFound();
            }

            FoodItem = await _context.FoodItems.FirstOrDefaultAsync(m => m.Id == id);

            if (FoodItem == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}