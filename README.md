
# FoodWeb

FoodWeb is a full-stack food e-commerce application built with .NET technologies. It demonstrates a scalable microservices architecture with secure APIs, an API Gateway, and a user-friendly web frontend — ideal as a portfolio project to showcase real-world backend and full-stack engineering skills.

## 🚀 Project Overview

FoodWeb simulates a complete online food store environment where users can browse products, add items to a cart, apply coupons, place orders, and more. The system is built using multiple microservices communicating through a unified API gateway.

### Objective:
The repository represents a large-scale, modular web application focused on selling food-related products. It implements all major e-commerce features like user registration, product management, shopping carts, orders, coupons, payments, and rewards.


## ⭐ Key Features:
- **Microservices Architecture**: The project is divided into several services, each responsible for a specific domain such as **ProductAPI**, **AuthAPI**, **ShoppingCartAPI**, **OrderAPI**, **CouponAPI**, **EmailAPI**, and **RewardAPI**.
- **APIs**: Each service exposes a REST API, and these APIs handle different aspects of the application. For example, `ProductAPI` manages products, `AuthAPI` handles user authentication, and `OrderAPI` processes orders.
- **Gateways and Message Bus**: The project uses **Ocelot Gateway** for API Gateway functionality, and there is a **MessageBus** for communication between services, likely using messaging protocols like Azure Service Bus.
- **Web Frontend**: The `Food.Web` project is the web frontend built using ASP.NET MVC, which serves the web pages that users interact with. The views are structured using Razor templates (CSHTML files), and the site relies on jQuery, Bootstrap, and other client-side libraries.
- **Services**: 
  - The backend services handle business logic such as managing products, handling orders, processing coupons, user authentication, and rewards.
  - **AuthService** is responsible for user authentication and authorization, using JWT for secure communication.
  - **ShoppingCartService**, **OrderService**, **CouponService**, and **ProductService** handle core e-commerce functionalities.
  
### Main Components:
1. **Food.Web**: The main frontend of the web application where users interact with the system.
2. **Microservices**:
   - **ProductAPI**: Manages products (CRUD operations).
   - **ShoppingCartAPI**: Manages shopping cart-related operations.
   - **OrderAPI**: Manages orders, including order processing and payments.
   - **CouponAPI**: Handles coupon management for discounts.
   - **AuthAPI**: Manages user authentication and authorization using Identity and JWT.
   - **RewardAPI**: Handles rewards for user purchases.
   - **EmailAPI**: Sends emails, potentially for notifications or confirmations.
3. **Data Models and DTOs**: Each microservice has its own database models and DTOs (Data Transfer Objects) for communication between services.
4. **API Gateway (Ocelot)**: Handles routing and request aggregation across multiple services.
5. **Utilities**: Common functionality shared across services, including static configuration details, extensions, and validation.
6. **Message Bus**: Likely responsible for asynchronous communication between services (e.g., event-driven architecture).
7. 
## 🛠️ Tech Stack
- **Backend**: ASP.NET Core, Entity Framework Core, SQL Server
- **Frontend**: ASP.NET MVC, Bootstrap, jQuery, Razor Views
- **Microservices**: ASP.NET Core APIs for each domain
- **Authentication**: JWT, ASP.NET Core Identity
- **Message Bus**: Azure Service Bus (for communication between microservices)
- **Database**: SQL Server with Entity Framework Core migrations
- **API Gateway**: Ocelot for managing API routing


## 🧱 Architecture

FoodWeb/
├── API Gateway (Ocelot) # Routes requests to microservices
├── Product Microservice # Product catalog and management
├── Shopping Cart Microservice # Cart operations
├── Order Microservice # Order placement and tracking
├── Coupon Microservice # Coupon validation and discounts
├── Reward Microservice # Reward point logic
├── Email Service # Email notifications
├── Auth Service # User authentication & JWT
├── Web Frontend (ASP.NET MVC) # User interface
└── README.md

## 📦 Getting Started

### Prerequisites

- .NET SDK (7 or later)
- SQL Server
- Visual Studio or VS Code

### Steps

1. **Clone the repo**
git clone https://github.com/mrshri/FoodWeb.git

markdown
Copy code
2. **Update database connection strings** in each service’s `appsettings.json`
3. **Apply migrations**
dotnet ef database update --project <service>.Infrastructure

markdown
Copy code
4. **Run the API Gateway and microservices**
5. **Start the web frontend**
dotnet run --project FoodWeb.Web

pgsql
Copy code

## 📌 Sample API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| /api/products | GET | Get all products |
| /api/cart | POST | Add item to cart |
| /api/orders | POST | Place an order |
| /api/coupons/apply | POST | Apply coupon to cart |
| /auth/login | POST | Authenticate and get JWT |

> Replace paths with actual routes defined in your projects.

## 🎯 What You’ll Learn

- Building distributed systems using microservices
- API gateway design patterns
- Secure API development with JWT
- Hands-on experience with ASP.NET Core ecosystem

## 📌 Future Improvements

- Add Swagger documentation for all APIs
- Implement async messaging between services
- Deploy using Docker & Kubernetes
- Add automated unit and integration tests

## 📄 License

This project is open-source — feel free to use and modify it!
