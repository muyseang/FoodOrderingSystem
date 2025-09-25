using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodOrderingSystem.Pages.Account
{
    public class SignOutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Clear all session data
            HttpContext.Session.Clear();
            
            // Redirect to home page
            return RedirectToPage("/Index");
        }
        
        public IActionResult OnPost()
        {
            // Clear all session data
            HttpContext.Session.Clear();
            
            // Redirect to home page
            return RedirectToPage("/Index");
        }
    }
}