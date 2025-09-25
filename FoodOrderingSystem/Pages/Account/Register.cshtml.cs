using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodOrderingSystem.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public new User User { get; set; } = new User();
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Hash the password
            User.Password = BCrypt.Net.BCrypt.HashPassword(User.Password);
            
            // Set default role if not specified
            if (string.IsNullOrEmpty(User.Role))
            {
                User.Role = "Customer";
            }

            _context.Users.Add(User);
            await _context.SaveChangesAsync();
            
            // Auto-login the user after successful registration
            HttpContext.Session.SetInt32("UserId", User.Id);
            HttpContext.Session.SetString("UserId", User.Id.ToString());
            HttpContext.Session.SetString("Username", User.Username);
            HttpContext.Session.SetString("Role", User.Role);
            
            return RedirectToPage("/Index");
        }
    }
}
