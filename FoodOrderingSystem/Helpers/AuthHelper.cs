using Microsoft.AspNetCore.Http;
namespace FoodOrderingSystem.Helpers
{
    public class AuthHelper
    {
        public static bool IsAdmin(HttpContext context)
        {
            return context.Session.GetString("Role") == "Admin";
        }

        public static bool IsCustomer(HttpContext context)
        {
            return context.Session.GetString("Role") == "Customer";
        }
    }
}
