# IIStudy WebServices

IIStudy is a learning-materials management and sales system built to centralize educational content in one place. The project provides students with access to study books, exams, solutions, events, reviews, personal purchases, and an administration platform for managing the system's data.

The system was developed as a full-stack educational project using ASP.NET Core, Web API services, WPF, Microsoft Access, and a layered architecture that separates the user interface, business logic, and database access.

## Main Idea

Students often need to search across many different websites to find books, exam examples, solutions, and relevant learning information. IIStudy solves this by creating one organized platform where users can browse learning materials, buy physical or digital books, view exams and solutions, track relevant events, and manage their personal study library.

## Main Features

### Guest Users

- Browse the public book catalog
- View book details and reviews
- Browse subjects, exams, and solutions
- Open exam and solution files
- View study-related events in a calendar
- Register and sign in

### Registered Users

- Add books to a shopping cart
- Purchase physical or online books
- View previous orders and order details
- Access purchased digital books
- Generate QR links for owned books
- Add reviews and ratings to books
- Like or dislike reviews
- Update profile details, profile image, and password

### Administrator

- Manage users and permissions
- Ban and unban users
- Promote or remove admin access
- Create, update, soft-delete, and restore books
- Manage subjects, exams, solutions, and events
- Manage reviews
- View orders and order details
- Update delivery status
- Manage stock and book availability

## System Architecture

IIStudy is divided into five main parts:

1. **User Website** – the public and registered-user interface.
2. **ASP.NET Core MVC Web App** – handles Razor pages, controllers, CSS, JavaScript, and communication with the API.
3. **ASP.NET Core Web API Service** – contains the main business logic and exposes endpoints to the clients.
4. **WPF Admin Application** – desktop management application for administrators.
5. **Microsoft Access Database** – stores the system data.

The clients do not access the database directly. Instead, the MVC web application and the WPF admin application communicate with the Web API service. The service validates requests, executes business logic, uses repositories to access the database, and returns structured responses.

```text
User Website / WPF Admin App
            |
            v
 ASP.NET Core Web API Service
            |
            v
 Repository Layer + OleDb
            |
            v
 Microsoft Access Database
```

## Layered Design

The project follows a layered structure:

- **Presentation Layer** – MVC web pages and WPF admin screens.
- **Business Logic Layer** – validation, permissions, purchase logic, cart logic, stock handling, and system rules.
- **Data Access Layer** – repository classes and OleDb database communication.

This separation keeps the database hidden from the clients and makes the project easier to maintain and extend.

## Database

The database is built with Microsoft Access and accessed through OleDb. It includes tables for:

- Registered users
- Books
- Subjects
- Exams
- Solutions
- Events
- Reviews
- Likes and dislikes
- Shopping carts
- Orders
- Order-book relations

## Technologies Used

- **C#** – main programming language
- **ASP.NET Core MVC** – web application and Razor pages
- **ASP.NET Core Web API** – backend service layer
- **WPF** – desktop administrator application
- **Microsoft Access** – database
- **OleDb** – database connection layer
- **SQL** – data queries and database operations
- **HTML / CSS / JavaScript** – web interface and client-side behavior
- **Razor** – dynamic server-rendered web pages
- **XAML** – WPF interface design
- **Swagger** – API testing
- **GitHub** – source control

## Backend Highlights

- Generic repository pattern
- Reflection-based model creation and shared CRUD logic
- Parameterized SQL queries to reduce SQL injection risks
- Password hashing with salt
- File upload support for PDFs and images
- External QR-code generation for digital book access
- Async web requests between the MVC client and the Web API
- Business validation before database changes

## Example Business Flows

### Sign Up

A new user submits registration details through the web application. The Web API validates the data, checks whether the username or email already exists, hashes the password, and stores the new user in the database.

### Add to Cart

A registered user selects a book and adds it to the shopping cart. The service checks that the book exists, verifies availability, and updates the user's cart.

### Purchase

The user completes an order from the cart. The backend validates the cart, calculates the total price, creates an order, connects books to the order, and updates the stock when needed.

### Admin Content Management

The administrator uses the WPF application to manage books, exams, solutions, events, users, reviews, and orders. All admin actions are sent to the Web API, which validates and applies the requested changes.

## Getting Started

### Requirements

- Visual Studio
- .NET SDK / ASP.NET Core runtime
- Microsoft Access Database Engine / ACE OleDb provider
- Windows environment for running the WPF admin application

### Running the Project

1. Clone the repository:

```bash
git clone https://github.com/Drako59/IIStudy-WebServices.git
```

2. Open the solution in Visual Studio.

3. Make sure the Access database path is configured correctly in the project settings / connection string.

4. Restore NuGet packages.

5. Run the Web API project.

6. Run the MVC Web App project.

7. Optional: run the WPF Admin application for system management.

## Notes

This project was created as an educational full-stack system for learning-material management, e-commerce logic, web services, database access, and multi-platform client development.

For real production use, the project would require additional security hardening, deployment configuration, production-grade database hosting, and integration with a real payment provider.

## Author

Developed by **Noam Kalderon**.
