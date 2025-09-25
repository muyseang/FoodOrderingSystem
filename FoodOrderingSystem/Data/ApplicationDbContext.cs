using FoodOrderingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // DbSets
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for prices
            modelBuilder.Entity<FoodItem>()
                .Property(f => f.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CartItem>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            // Configure CartItem relationships
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.FoodItem)
                .WithMany()
                .HasForeignKey(c => c.FoodItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed admin user
            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Id = 1,
                   Username = "admin",
                   Password = "$2a$12$DCFivOnraf6V7aeMTeey8e9v9nRbetjV1ebRQTNn4fAuBxv2qyajq", // bcrypt hash for "admin123"
                   Role = "Admin"
               }
            );

            // Seed sample food items with images
            modelBuilder.Entity<FoodItem>().HasData(
                new FoodItem
                {
                    Id = 1,
                    Name = "Classic Cheeseburger",
                    Description = "Juicy beef patty with melted cheese, lettuce, tomato, and our special sauce on a toasted bun.",
                    Price = 12.99m,
                    Category = "Burgers",
                    ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ca4b?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 2,
                    Name = "Margherita Pizza",
                    Description = "Traditional pizza with fresh mozzarella, tomatoes, basil, and olive oil on a crispy thin crust.",
                    Price = 15.50m,
                    Category = "Pizza",
                    ImageUrl = "https://images.unsplash.com/photo-1604382355076-af4b0eb60143?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 3,
                    Name = "Caesar Salad",
                    Description = "Crisp romaine lettuce with parmesan cheese, croutons, and our homemade Caesar dressing.",
                    Price = 9.99m,
                    Category = "Salads",
                    ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 4,
                    Name = "Grilled Chicken Breast",
                    Description = "Tender grilled chicken breast seasoned with herbs, served with roasted vegetables.",
                    Price = 18.75m,
                    Category = "Main Courses",
                    ImageUrl = "https://images.unsplash.com/photo-1532550907401-a500c9a57435?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 5,
                    Name = "Fish & Chips",
                    Description = "Beer-battered fish fillets with golden fries and mushy peas, served with tartar sauce.",
                    Price = 16.25m,
                    Category = "Main Courses",
                    ImageUrl = "https://images.unsplash.com/photo-1544025162-d76694265947?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 6,
                    Name = "Pepperoni Pizza",
                    Description = "Classic pizza topped with spicy pepperoni and mozzarella cheese on our signature dough.",
                    Price = 17.00m,
                    Category = "Pizza",
                    ImageUrl = "https://images.unsplash.com/photo-1513104890138-7c749659a591?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 7,
                    Name = "Chocolate Brownie",
                    Description = "Rich, fudgy chocolate brownie served warm with vanilla ice cream and chocolate sauce.",
                    Price = 7.50m,
                    Category = "Desserts",
                    ImageUrl = "https://images.unsplash.com/photo-1606313564200-e75d5e30476c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 8,
                    Name = "Chicken Wings",
                    Description = "Crispy chicken wings tossed in your choice of buffalo, BBQ, or honey mustard sauce.",
                    Price = 11.99m,
                    Category = "Appetizers",
                    ImageUrl = "https://images.unsplash.com/photo-1567620832903-9fc6debc209f?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 9,
                    Name = "Veggie Burger",
                    Description = "House-made plant-based patty with avocado, sprouts, and chipotle mayo on a whole grain bun.",
                    Price = 13.50m,
                    Category = "Burgers",
                    ImageUrl = "https://images.unsplash.com/photo-1520072959219-c595dc870360?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 10,
                    Name = "Tiramisu",
                    Description = "Classic Italian dessert with layers of coffee-soaked ladyfingers and mascarpone cream.",
                    Price = 8.99m,
                    Category = "Desserts",
                    ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 11,
                    Name = "Greek Salad",
                    Description = "Fresh mixed greens with feta cheese, olives, tomatoes, and cucumber in olive oil dressing.",
                    Price = 10.75m,
                    Category = "Salads",
                    ImageUrl = "https://images.unsplash.com/photo-1540420773420-3366772f4999?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                },
                new FoodItem
                {
                    Id = 12,
                    Name = "Loaded Nachos",
                    Description = "Crispy tortilla chips loaded with cheese, jalapeños, sour cream, and guacamole.",
                    Price = 9.50m,
                    Category = "Appetizers",
                    ImageUrl = "https://images.unsplash.com/photo-1513456852971-30c0b8199d4d?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80"
                }
            );
        }
    }
}
