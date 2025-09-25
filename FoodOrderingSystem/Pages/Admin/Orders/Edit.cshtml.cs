using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;

namespace FoodOrderingSystem.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!AuthHelper.IsAdmin(HttpContext))
                return RedirectToPage("/Account/Login");

            Order = await _context.Orders.FindAsync(id);
            if (Order == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!AuthHelper.IsAdmin(HttpContext))
                return RedirectToPage("/Account/Login");

            var orderToUpdate = await _context.Orders.FindAsync(Order.Id);
            if (orderToUpdate == null)
                return NotFound();

            orderToUpdate.Status = Order.Status;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
