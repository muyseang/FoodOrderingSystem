using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Helpers;

namespace FoodOrderingSystem.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Order> Orders { get; set; }

        [BindProperty(SupportsGet = true)]
        public string PaymentStatus { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!AuthHelper.IsAdmin(HttpContext))
                return RedirectToPage("/Account/Login");

            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            if (!string.IsNullOrEmpty(PaymentStatus))
                query = query.Where(o => o.PaymentStatus == PaymentStatus);

            Orders = await query.ToListAsync();
            return Page();
        }
    }
}
