using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Pages.Admin.Orders
{
    public class MyordersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Order> Orders { get; set; }

        public MyordersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) { Orders = new List<Order>(); return; }

            Orders = await _context.Orders
                .Where(o => o.UserId == userId.Value)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
