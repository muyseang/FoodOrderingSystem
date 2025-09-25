using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User CurrentUser { get; set; } = new User();
        
        [BindProperty]
        public string? NewPassword { get; set; }
        
        [BindProperty]
        public string? ConfirmPassword { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
            
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            CurrentUser = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
            
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Update username
            user.Username = CurrentUser.Username;

            // Update password if provided
            if (!string.IsNullOrEmpty(NewPassword))
            {
                if (NewPassword != ConfirmPassword)
                {
                    ErrorMessage = "New password and confirmation password do not match.";
                    CurrentUser = user;
                    return Page();
                }

                if (NewPassword.Length < 6)
                {
                    ErrorMessage = "Password must be at least 6 characters long.";
                    CurrentUser = user;
                    return Page();
                }

                // Hash the new password
                user.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                
                // Update session username
                HttpContext.Session.SetString("Username", user.Username);
                
                SuccessMessage = "Profile updated successfully!";
                CurrentUser = user;
                NewPassword = null;
                ConfirmPassword = null;
            }
            catch (Exception)
            {
                ErrorMessage = "An error occurred while updating your profile.";
                CurrentUser = user;
            }

            return Page();
        }
    }
}