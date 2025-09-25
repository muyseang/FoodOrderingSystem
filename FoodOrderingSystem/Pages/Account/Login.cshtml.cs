using FoodOrderingSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/Index";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }

            // Set session data
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            
            // Redirect to return URL if provided, otherwise go to appropriate default page
            if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            
            // Redirect based on role
            if (user.Role == "Admin")
            {
                return RedirectToPage("/Admin/Index");
            }
            
            return RedirectToPage("/Index");
        }
    }
}
