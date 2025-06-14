
# Assignment 03: Event Management Application with Real-Time Updates

## Introduction
This project involves the development of a web application for event management, incorporating real-time updates using SignalR. The application is designed to provide a robust and versatile system for creating, viewing, editing, and deleting events. Additional features include user management, event registration, and statistical reporting. The technical implementation leverages ASP.NET, Entity Framework Core, and SQL Server to ensure efficient database management and application logic. The application prioritizes a user-friendly interface, data security, and seamless real-time user-system interactions.

## Assignment Objectives
The primary objectives of this assignment are as follows:
- Utilize Visual Studio .NET to create an ASP.NET MVC project.
- Implement CRUD (Create, Read, Update, Delete) operations using Entity Framework Core, integrated with real-time communication via SignalR.
- Apply data type validation to all fields to ensure data integrity.
- Execute the project and conduct thorough testing of ASP.NET application actions.

## Database Design
The database schema is structured to support the application's core functionalities. It includes the following tables with their respective relationships:
- **Users**: Manages user authentication and profiles (e.g., UserID, Username, Password, FullName, Email, Role).
- **Events**: Stores event details (e.g., EventID, Title, StartTime, EndTime, CategoryID).
- **EventCategories**: Categorizes events (e.g., CategoryID, CategoryName).
- **Attendees**: Tracks event registrations (e.g., AttendeeID, UserID, EventID, Name, Email, RegistrationTime).
Relationships are established to reflect a many-to-one association between Events and EventCategories, and one-to-many associations between Users and Attendees, as well as Events and Attendees.

## Main Functions
The application encompasses the following key functionalities:
- **User Management**: Supports sign-in, sign-up, profile management, and role-based access control.
- **Event Management**: Enables the creation, viewing, editing, and deletion of events, including details such as title, description, time, location, and category.
- **Event Registration**: Allows users to register for events by providing their name and email.
- **Real-Time Updates**: Implements SignalR to provide real-time updates on event information and attendee counts.
- **Search and Filter Events**: Facilitates event searching by name and category, with filtering options based on date.
- **Reports and Analytics**: Generates reports on event counts, attendee numbers, and participation trends for analytical purposes.

## Technical Requirements
- **Framework**: ASP.NET with MVC architecture.
- **ORM**: Entity Framework Core for database operations.
- **Database**: SQL Server for data persistence.
- **Real-Time Communication**: SignalR for real-time updates.
- **Development Environment**: Visual Studio .NET.

## 3-Layer Architecture Design
The application follows a 3-layer architecture to ensure separation of concerns and maintainability:
- **Presentation Layer**: 
  - Comprises the ASP.NET MVC controllers, views, and Razor pages.
  - Handles user interface logic, including event management forms, user authentication pages, and real-time update displays via SignalR hubs.
  - Location: `/Controllers`, `/Views`, `/wwwroot`.
- **Business Logic Layer (BLL)**:
  - Contains services and business logic to manage application workflows, such as event creation, user registration, and report generation.
  - Includes interfaces and implementations for dependency injection.
  - Location: `/Services`.
- **Data Access Layer (DAL)**:
  - Manages database interactions using Entity Framework Core.
  - Includes the `DbContext`, entity models, and repository patterns for CRUD operations.
  - Location: `/Data`, `/Models`.

This architecture facilitates modular development, testing, and scalability, with each layer interacting through defined interfaces.

## Project File Structure
The project is organized as follows to align with the 3-layer architecture and best practices:
```
EventManagementApp/
│
├── /Controllers/              # MVC controllers for handling HTTP requests
│   ├── HomeController.cs
│   ├── AccountController.cs
│   ├── EventController.cs
│   └── SignalRHubController.cs
│
├── /Views/                   # Razor views for the presentation layer
│   ├── Home/
│   ├── Account/
│   ├── Events/
│   └── Shared/
│
├── /wwwroot/                 # Static files (CSS, JS, images)
│   ├── css/
│   ├── js/
│   └── lib/
│
├── /Services/                # Business logic layer
│   ├── Interfaces/           # Service interfaces
│   │   ├── IEventService.cs
│   │   └── IUserService.cs
│   ├── Implementations/      # Service implementations
│   │   ├── EventService.cs
│   │   └── UserService.cs
│
├── /Data/                    # Data access layer
│   ├── ApplicationDbContext.cs  # Entity Framework Core DbContext
│   ├── Migrations/           # Database migration files
│   └── Repositories/         # Repository patterns
│       ├── IRepository.cs
│       └── EventRepository.cs
│
├── /Models/                  # Entity models
│   ├── User.cs
│   ├── Event.cs
│   ├── EventCategory.cs
│   └── Attendee.cs
│
├── /Hubs/                    # SignalR hubs for real-time updates
│   └── EventHub.cs
│
├── appsettings.json          # Configuration file for connection strings and settings
├── Program.cs                # Application entry point
├── Startup.cs                # Configuration for services and middleware
└── EventManagementApp.csproj # Project file
```

## Setup and Installation
1. Clone the repository to your local machine.
2. Ensure SQL Server is installed and configured.
3. Open the solution in Visual Studio .NET.
4. Update the connection string in the `appsettings.json` file to point to your SQL Server instance.
5. Restore NuGet packages and build the solution.
6. Apply any pending migrations using Entity Framework Core (e.g., `Update-Database` via Package Manager Console).
7. Run the application and navigate to the specified URL.

