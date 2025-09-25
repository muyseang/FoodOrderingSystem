using System;
using System.Collections.Generic;
namespace FoodOrderingSystem.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pending, Paid, InProgress, Completed
        public List<OrderItem> OrderItems { get; set; }
        public string PaymentStatus { get; set; } // Pending, Paid
    }
}
