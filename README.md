# Employee Management System

## Overview

The Employee Management System is a robust, flexible, and user-friendly application developed using .NET Core for backend services and plain HTML, CSS, and JavaScript for the frontend. The system incorporates features that ensure reusability, security, and functionality, with a focus on token-based authentication and CRUD operations.

## Features

- Token-based authentication using JWT
- APIs for CRUD operations (Create, Read, Update, Delete)
- Secure backend-API integration
- User-friendly frontend interface

## Backend Architecture

### Technology Stack

- **Framework**: .NET Core
- **Database**: SQL Server
- **Authentication**: JSON Web Tokens (JWT)

### Project Structure

The backend project is structured as follows:

- **Controllers**: Handle incoming HTTP requests and return responses.
- **Models**: Define the data structures.
- **Data**: Manage database context and migrations.
- **Services**: Contain business logic, including user authentication and password hashing.

### Web API

The Web API provides endpoints for managing employees, departments, roles, and user authentication.

#### Endpoints

- **Authentication**:
- `POST /api/auth/login`: Authenticate user and return JWT.

- **Users**:
- `GET /api/users`: Get all users.
- `GET /api/users/{id}`: Get user by ID.
- `POST /api/users`: Create a new user.
- `PUT /api/users/{id}`: Update user by ID.
- `DELETE /api/users/{id}`: Delete user by ID.

- **Employees**:
- `GET /api/employees`: Get all employees.
- `GET /api/employees/{id}`: Get employee by ID.
- `POST /api/employees`: Create a new employee.
- `PUT /api/employees/{id}`: Update employee by ID.
- `DELETE /api/employees/{id}`: Delete employee by ID.

### Database Schema

The database schema includes the following tables:

- **Users**: Stores user information with password hash and salt.
- **Employees**: Stores employee information.
- **Departments**: Stores department information.
- **Roles**: Stores role information.

### Controllers

- **AuthController**: Manages user authentication.
- **UsersController**: Handles CRUD operations for users.
- **EmployeesController**: Handles CRUD operations for employees.
- **DepartmentsController**: Handles CRUD operations for departments.
- **RolesController**: Handles CRUD operations for roles.

### JWT Authentication

JWT authentication is implemented to secure the API endpoints. Users must authenticate to obtain a token, which is then used to access protected endpoints.

#### Key Configuration

In `appsettings.json`:

```json
"Jwt": {
"Key": "yourSecretKey",
"Issuer": "yourIssuer",
"Audience": "yourAudience"
}


