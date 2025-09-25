# 🍽️ Food Ordering System

A comprehensive food ordering web application built with **ASP.NET Core 9.0** and **Entity Framework Core**, featuring user authentication, cart management, and demo payment processing.

## 🚀 Features

### 👤 User Management
- **User Registration & Login** with secure password hashing (BCrypt)
- **User Profile Management** with password change functionality
- **Role-based Access Control** (User/Admin roles)
- **Session Management** for persistent login state

### 🛒 Shopping Experience
- **Browse Food Menu** with categories and real food images
- **User-Specific Cart System** (database-driven)
- **Real-time Cart Updates** with AJAX operations
- **Login Required** for cart operations

### 💳 Checkout & Payments
- **Demo Payment System** with multiple options:
  - Demo Visa & MasterCard
  - **ABA Bank Cambodia** (mobile banking simulation)
  - Demo PayPal
- **Tax Calculation** (10% VAT)
- **Order Confirmation** with detailed receipts

### 🔧 Admin Features
- **Food Item Management** (CRUD operations)
- **Order Management** and tracking
- **User Management** capabilities

### 🎨 UI/UX
- **Bootstrap 5** responsive design
- **Font Awesome** icons
- **Modern Interface** with animations and transitions
- **Mobile-Friendly** design

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: Entity Framework Core 9.0 with SQL Server
- **Authentication**: Custom implementation with BCrypt
- **Frontend**: Bootstrap 5, HTML5, CSS3, JavaScript
- **Icons**: Font Awesome 6
- **Development**: Visual Studio Code compatible

## 📋 Prerequisites

- **.NET 9.0 SDK** or later
- **SQL Server** or **SQL Server Express**
- **Visual Studio** or **Visual Studio Code** (recommended)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd FoodOrderingSystem
```

### 2. Database Setup
```bash
cd FoodOrderingSystem
dotnet ef database update
```

### 3. Run the Application
```bash
dotnet run
```

### 4. Access the Application
- **URL**: `http://localhost:5211`
- **Default Admin**: Username: `admin`, Password: `admin123`

## 📁 Project Structure

```
FoodOrderingSystem/
├── Data/                           # Database context and configurations
├── Models/                         # Entity models (User, FoodItem, CartItem, Order)
├── Pages/                          # Razor Pages
│   ├── Account/                    # User authentication pages
│   ├── Admin/                      # Admin management pages
│   ├── Cart/                       # Shopping cart functionality
│   ├── Menu/                       # Food menu display
│   └── Shared/                     # Shared layouts and components
├── Migrations/                     # Entity Framework migrations
├── Helpers/                        # Utility classes and extensions
└── wwwroot/                        # Static files (CSS, JS, images)
```

## 🗃️ Database Schema

### Core Tables
- **Users**: User accounts with authentication
- **FoodItems**: Menu items with categories and prices
- **CartItems**: User-specific shopping cart (database-driven)
- **Orders**: Order tracking and management
- **OrderItems**: Individual items within orders

## 🌟 Key Features Implemented

### 🔐 Security
- **Password Encryption**: BCrypt hashing algorithm
- **Session Management**: Secure user state management
- **Input Validation**: Form validation and SQL injection prevention
- **Authentication Required**: Protected cart and profile operations

### 🛒 Cart System
- **User-Specific Carts**: Each user has their own cart stored in database
- **Persistent Cart**: Cart items survive browser sessions
- **Real-time Updates**: AJAX-powered quantity changes and item removal
- **Login Validation**: Must be logged in to add items to cart

### 💰 Demo Payment System
- **Multiple Payment Methods**: Cards, ABA Bank, PayPal simulation
- **Payment Processing Animation**: 3-step processing simulation
- **Tax Calculation**: Fixed 10% VAT applied to all orders
- **Order Confirmation**: Professional receipt with order details

### 🇰🇭 Cambodian Localization
- **ABA Bank Integration**: Popular Cambodian mobile banking simulation
- **Local Phone Format**: +855 country code with proper formatting
- **Currency**: USD pricing (standard in Cambodia)

## 🎯 Demo Credentials

### Users
- **Admin**: `admin` / `admin123`
- **Test User**: Register new account or use existing demo user

### Demo Payment
- **ABA Bank**: Phone: `+855 12 345 678`, PIN: `123456`
- **Demo Cards**: Pre-filled with test card numbers
- **PayPal**: `demo@paypal.com` / `demo123`

## 📱 Pages Available

| Page | URL | Description |
|------|-----|-------------|
| **Home** | `/` | Welcome page with system overview |
| **Menu** | `/Menu` | Browse food items and add to cart |
| **Cart** | `/Cart` | View and manage cart items |
| **Checkout** | `/Admin/Orders/Checkout` | Process demo payments |
| **Profile** | `/Account/Profile` | Manage user profile |
| **Admin Panel** | `/Admin/*` | Food and order management |
| **Privacy Policy** | `/Privacy` | Comprehensive privacy information |

## 🔧 Development Notes

### Environment Configuration
- **Development**: Uses local SQL Server
- **Connection String**: Configured in `appsettings.json`
- **Migrations**: Auto-applied on startup in development

### Demo Data
- **Seeded Food Items**: 20+ food items with real images
- **Admin User**: Pre-created admin account
- **Sample Categories**: Main Course, Appetizer, Dessert, Beverage

## 🎨 UI Theme
- **Color Scheme**: Bootstrap primary colors with custom accents
- **Typography**: Modern, readable fonts
- **Components**: Cards, modals, buttons with hover effects
- **Responsive**: Mobile-first design approach

## 📄 License
This project is for educational and demonstration purposes.

## 🤝 Contributing
This is a demo project. For educational use and learning purposes.

---

**Built with ❤️ using ASP.NET Core 9.0**