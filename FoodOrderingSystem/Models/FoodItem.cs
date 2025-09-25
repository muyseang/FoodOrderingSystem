using System.ComponentModel.DataAnnotations;
namespace FoodOrderingSystem.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        
        [Required]
        public string Category { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }
    }
}
