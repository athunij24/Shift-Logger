# Shift Logger Project

## Overview

The Shift Logger Project is designed to manage and log employee shifts. The project consists of a web API for backend operations and a console application for user interaction.

- **Web API**: Provides RESTful endpoints for managing employees and shifts.
- **Console Application**: A CLI-based application that interacts with the API to register employees, log shifts, view shifts, update, and delete shifts.

## Project Structure

### 1. Web API

The web API is built using ASP.NET Core and EF Core. It includes the following components:

- **Controllers**: Handle HTTP requests and responses.
- **Services**: Contain business logic and interact with the data access layer.
- **Repositories**: Interface with the database for CRUD operations.
- **Models**: Define the data structure for employees and shifts.
- **DbContext**: Manages the database connection and entity mapping.

### RESTful API Endpoints

The API follows REST principles, providing a uniform and stateless interface for interacting with resources. Here are the key endpoints:

- **Register Employee**
  - **Method**: `POST`
  - **URL**: `/api/Employees/register`
  - **Description**: Creates a new employee.
  - **Body**: `{"UserName": "string", "Password": "string", "Role": "string"}`

- **Login Employee**
  - **Method**: `POST`
  - **URL**: `/api/Employees/login`
  - **Description**: Authenticates an employee.
  - **Body**: `{"UserName": "string", "Password": "string"}`

- **Get Shifts**
  - **Method**: `GET`
  - **URL**: `/api/Shifts`
  - **Description**: Retrieves a list of all shifts.

- **Get Shift by ID**
  - **Method**: `GET`
  - **URL**: `/api/Shifts/{id}`
  - **Description**: Retrieves a specific shift by its ID.

- **Update Shift**
  - **Method**: `PUT`
  - **URL**: `/api/Shifts/{id}`
  - **Description**: Updates a specific shift.
  - **Body**: `{"StartTime": "string", "EndTime": "string", "EmployeeId": "long"}`

- **Delete Shift**
  - **Method**: `DELETE`
  - **URL**: `/api/Shifts/{id}`
  - **Description**: Deletes a specific shift.

### 2. Console Application

The console application interacts with the web API to perform various operations related to employee shift management. It includes:

- **EmployeeManager**: Handles user interactions and communicates with the API.
- **User Interface**: Provides a command-line interface for users to perform actions like logging shifts, registering employees, etc.
