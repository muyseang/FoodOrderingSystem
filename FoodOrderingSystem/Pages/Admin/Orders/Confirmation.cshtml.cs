using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;

namespace FoodOrderingSystem.Pages.Admin.Orders
{
    public class ConfirmationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ConfirmationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }

        public async Task OnGetAsync(int id)
        {
            Order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
