Employee Management System is a full-stack web application developed to manage employee information efficiently. The system provides a complete solution for creating, updating, retrieving, and deleting employee records through a user-friendly interface connected with a secure backend API.

The project consists of three main layers:

Frontend:
The frontend is developed using Angular. It provides an interactive and responsive user interface for managing employee data. Angular components and services are used to handle user interactions and communicate with the backend API using HttpClient.

Backend:
The backend is developed using ASP.NET Core Web API with C#. It is responsible for handling API requests, implementing business logic, and managing communication between the frontend and the database. The project follows a layered architecture using Controllers, Services, and Interfaces to achieve better organization, maintainability, and separation of responsibilities.

Database:
The system uses Oracle Database for storing and managing employee information. Entity Framework Core with Oracle Entity Framework Core Provider is used to connect the application with the database and perform database operations through DbContext.

The application supports complete CRUD operations:

Creating new employee records.
Retrieving employee information.
Updating existing employee data.
Deleting employee records.

The project also implements important software development practices, including:

Dependency Injection for managing application services.
RESTful API architecture for communication between frontend and backend.
JWT Authentication for securing API access.
Swagger for API documentation and testing.
Serilog for application logging and error tracking.
Entity Framework Core for database management and integration with Oracle Database.

The main goal of this project is to build a scalable and maintainable employee management system while applying modern full-stack development concepts and clean architecture principles.
