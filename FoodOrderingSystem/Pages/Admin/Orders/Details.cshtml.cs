using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Order Order { get; set; }

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!AuthHelper.IsAdmin(HttpContext))
                return RedirectToPage("/Account/Login");

            Order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (Order == null)
                return NotFound();

            return Page();
        }
    }
}
