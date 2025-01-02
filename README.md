
# To-Do Application

## Overview
This is a full-stack To-Do application designed as part of the **Enterprise Systems & Enterprise Application Development** coursework. It integrates a Blazor WebAssembly frontend with an ASP.NET Core Web API backend and uses MS SQL Server for data storage. The application implements secure JWT-based authentication and follows best practices for enterprise application development.

---

## Features

### Frontend (Blazor WebAssembly)
1. **Home Page**:
   - Displays all To-Do items with options to view, edit, and delete.
2. **Add To-Do Page**:
   - A form for adding new To-Do items.
3. **Edit To-Do Page**:
   - A form for updating existing To-Do items.
4. **Details Page**:
   - Displays the details of a specific To-Do item.
5. **Login Page**:
   - Authenticates users and retrieves a JWT token.
6. **Signup Page**:
   - Registers new users with username, email, and password.

### Backend (ASP.NET Core Web API)
1. **CRUD Operations**:
   - Add, edit, delete, and retrieve To-Do items.
2. **User Authentication**:
   - Register and login functionality with JWT-based authentication.
3. **Authorization**:
   - Ensures users can only manage their own To-Do items.

### Database (MS SQL Server with Entity Framework Core)
- **User Table**:
  - Stores user details (ID, Username, Email, PasswordHash).
- **TodoItem Table**:
  - Stores To-Do items linked to users with a foreign key.

---

## API Endpoints

### Authentication
1. `POST /api/auth/signup`
   - Registers a new user.
2. `POST /api/auth/login`
   - Authenticates a user and returns a JWT token.

### To-Do Items
1. `GET /api/todos`
   - Retrieves all To-Do items for the authenticated user.
2. `GET /api/todos/{id}`
   - Retrieves a specific To-Do item by ID.
3. `POST /api/todos`
   - Adds a new To-Do item.
4. `PUT /api/todos/{id}`
   - Updates an existing To-Do item.
5. `DELETE /api/todos/{id}`
   - Deletes a To-Do item by ID.

---

## Technologies Used

### Frontend
- **Framework**: Blazor WebAssembly
- **Libraries**:
  - Blazored.LocalStorage
  - Bootstrap for responsive design

### Backend
- **Framework**: ASP.NET Core Web API
- **Authentication**: Microsoft.AspNetCore.Authentication.JwtBearer
- **Password Hashing**: BCrypt.Net

### Database
- **MS SQL Server**: Entity Framework Core for database management

---

## Setup and Installation

### Prerequisites
1. .NET SDK (7.0 or later)
2. Visual Studio or VS Code
3. MS SQL Server
4. Postman (optional, for testing API endpoints)

### Backend Setup
1. Clone the repository.
2. Navigate to the backend project folder:
   ```bash
   cd ToDoApp.Api
   ```
3. Update the `appsettings.json` file with your database connection string:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ToDoAppDb;Trusted_Connection=True;"
   },
   "JwtSettings": {
       "SecretKey": "YourSuperSecretKeyHere123!"
   }
   ```
4. Run database migrations:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
5. Start the backend:
   ```bash
   dotnet run
   ```

### Frontend Setup
1. Navigate to the frontend project folder:
   ```bash
   cd ToDoApp.Client
   ```
2. Install dependencies (if required):
   ```bash
   dotnet restore
   ```
3. Start the frontend:
   ```bash
   dotnet run
   ```

---

## Testing

### API Testing
- Use Postman or Swagger (enabled in development mode) to test API endpoints.
- Ensure the `Authorization` header is set with the Bearer token for authenticated requests.

### Frontend Testing
- Open the application in your browser (default: `https://localhost:5001`).
- Test the following:
  - User signup and login.
  - Adding, editing, viewing, and deleting To-Do items.

---

## Future Improvements
- Add token refresh functionality to handle token expiration.
- Enhance UI/UX with advanced animations and dark mode.
- Implement a priority system for To-Do items.

---

## Conclusion
The To-Do application demonstrates the use of modern technologies to build a secure, scalable, and user-friendly enterprise system. It follows best practices in software development, including database management, authentication, and responsive design.
