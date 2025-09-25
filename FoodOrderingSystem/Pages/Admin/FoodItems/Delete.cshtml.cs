using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Admin.FoodItems
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FoodItem FoodItem { get; set; } = new FoodItem();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Check if user is logged in as admin
            if (!AuthHelper.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { returnUrl = $"/Admin/FoodItems/Delete?id={id}" });
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            // Check if user is logged in as admin
            if (!AuthHelper.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { returnUrl = "/Admin/FoodItems" });
            }

            if (id == null)
            {
                return NotFound();
            }

            FoodItem = await _context.FoodItems.FindAsync(id);

            if (FoodItem != null)
            {
                _context.FoodItems.Remove(FoodItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}