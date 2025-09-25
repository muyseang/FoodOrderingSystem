using FoodOrderingSystem.Data;
using FoodOrderingSystem.Helpers;
using FoodOrderingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Admin.Orders
{
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private const decimal TAX_RATE = 0.10m; // 10% VAT

        public CheckoutModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartItem> Cart { get; set; } = new List<CartItem>();
        [BindProperty]
        public string PaymentInfo { get; set; } = string.Empty; // Fake payment info

        public decimal Subtotal => Cart?.Sum(i => i.Price * i.Quantity) ?? 0;
        public decimal Tax => Subtotal * TAX_RATE;
        public decimal Total => Subtotal + Tax;

        public async Task OnGetAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                Cart = new List<CartItem>();
                return;
            }

            // Load cart from database
            Cart = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToPage("/Account/Login");
            }

            // Load cart from database
            Cart = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();
                
            if (Cart.Count == 0) return Page();

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = Total, // Now includes tax
                Status = "Pending",
                PaymentStatus = "Paid",
                OrderItems = Cart.Select(i => new OrderItem
                {
                    FoodItemId = i.FoodItemId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            
            // Clear user's cart after order is placed
            _context.CartItems.RemoveRange(Cart);
            await _context.SaveChangesAsync();

            return RedirectToPage("Confirmation", new { id = order.Id });
        }
    }
}
