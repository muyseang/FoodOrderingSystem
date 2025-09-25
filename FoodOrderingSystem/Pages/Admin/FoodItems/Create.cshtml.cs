using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;

namespace FoodOrderingSystem.Pages.Admin.FoodItems
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FoodItem FoodItem { get; set; }

        public IActionResult OnGet()
        {
            // Only allow admin
            if (!AuthHelper.IsAdmin(HttpContext))
                return RedirectToPage("/Account/Login");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!AuthHelper.IsAdmin(HttpContext))
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid)
                return Page();

            _context.FoodItems.Add(FoodItem);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
