using System.ComponentModel.DataAnnotations;

namespace FoodOrderingSystem.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }
        
        [Required]
        public int FoodItemId { get; set; }
        public FoodItem? FoodItem { get; set; }
        
        [Required]
        public string FoodName { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
