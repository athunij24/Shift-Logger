# Shift Logger Project

## Overview

The Shift Logger Project is designed to manage and log employee shifts. The project consists of a web API for backend operations and a console application for user interaction.

- **Web API**: Provides endpoints for managing employees and shifts.
- **Console Application**: A CLI-based application that interacts with the API to register employees, log shifts, view shifts, update, and delete shifts.

## Project Structure

### 1. Web API

The web API is built using ASP.NET Core and EF Core. It includes the following components:

- **Controllers**: Handle HTTP requests and responses.
- **Services**: Contain business logic and interact with the data access layer.
- **Repositories**: Interface with the database for CRUD operations.
- **Models**: Define the data structure for employees and shifts.
- **DbContext**: Manages the database connection and entity mapping.

### 2. Console Application

The console application interacts with the web API to perform various operations related to employee shift management. It includes:

- **EmployeeManager**: Handles user interactions and communicates with the API.
- **User Interface**: Provides a command-line interface for users to perform actions like logging shifts, registering employees, etc.




