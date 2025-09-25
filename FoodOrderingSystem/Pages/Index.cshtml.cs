using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace FoodOrderingSystem.Pages
{
    public class IndexModel : PageModel
    {

        public IActionResult OnGet()
        {
            return RedirectToPage("/Menu/Index");
        }
    }
}
