using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Admin.FoodItems
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
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
                return RedirectToPage("/Account/Login", new { returnUrl = $"/Admin/FoodItems/Edit?id={id}" });
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

        public async Task<IActionResult> OnPostAsync()
        {
            // Check if user is logged in as admin
            if (!AuthHelper.IsAdmin(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { returnUrl = "/Admin/FoodItems" });
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FoodItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(FoodItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.Id == id);
        }
    }
}