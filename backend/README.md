# Student Management API

REST API for student management developed in .NET 9 using Clean Architecture principles.

## Architecture

The project follows Clean Architecture principles with the following layers:

- **Domain**: Entities, interfaces, and domain exceptions
- **Application**: Use cases, DTOs, and application services
- **Infrastructure**: Repository implementations, Entity Framework context, base repository, configurations, migrations, and IoC
- **API**: Controllers and application configuration

## Technologies

- .NET 9
- Entity Framework Core
- PostgreSQL
- Swagger/OpenAPI
- xUnit (tests)
- Moq (mocking)
- FluentAssertions (assertions)

## Prerequisites

- .NET 9 SDK
- PostgreSQL
- Docker (optional)

## Setup

1. **Install dependencies:**
   ```bash
   dotnet restore
   ```

2. **Configure database:**
   - Install PostgreSQL
   - Create a database named `studentmanagement`
   - Update the connection string in `appsettings.json` if needed

3. **Run migrations:**
   ```bash
   dotnet ef database update
   ```

## Running the Project

1. **Development:**
   ```bash
   cd StudentManagement.API
   dotnet run
   ```

2. **Production:**
   ```bash
   dotnet build
   dotnet run --project StudentManagement.API
   ```

## Running Tests

```bash
cd ..
dotnet test
```

## API Endpoints

### Students

- `GET /api/students` - List all students
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create new student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

## API Documentation

Access `https://localhost:7001/swagger` for interactive API documentation.

## Project Structure

```
StudentManagement/
├── StudentManagement.API/              # Presentation layer
│   ├── Controllers/                   # REST Controllers
│   ├── Program.cs                     # App configuration
│   └── appsettings.json               # Settings
├── StudentManagement.Application/     # Application layer
│   ├── DTOs/                          # Data transfer objects
│   ├── Interfaces/                    # Service interfaces
│   └── Services/                      # Service implementations
├── StudentManagement.Domain/          # Domain layer
│   ├── Entities/                      # Domain entities
│   ├── Interfaces/                    # Repository interfaces
│   └── Exceptions/                    # Domain exceptions
├── StudentManagement.Infrastructure/  # Infrastructure layer
│   ├── Contexts/                      # EF Contexts and Migrations
│   │   ├── ApplicationDbContext.cs
│   │   ├── Configurations/            # Entity configurations
│   │   └── Migrations/                # EF migrations
│   ├── Base/                          # Generic repository base
│   ├── Repositories/                  # Repository implementations
│   └── IoC.cs                         # Dependency injection setup
└── StudentManagement.Tests/           # Tests
    ├── Unit/                          # Unit tests
    │   └── Application/
    └── Integration/                   # Integration tests
        └── Infrastructure/
```

## Validations

- **Name**: Required, max 100 characters
- **Email**: Required, valid format, max 100 characters, unique
- **RA**: Required, max 20 characters, unique, not editable
- **CPF**: Required, max 14 characters, unique, not editable

## Error Handling

- **400 Bad Request**: Invalid data
- **404 Not Found**: Student not found
- **409 Conflict**: Duplicate data (RA, CPF, Email)
- **500 Internal Server Error**: Internal server error

## Tests

The project includes unit and integration tests for:
- Student creation
- Student update
- Student deletion
- Business validations
- Exception handling

## Future Improvements

- Authentication and authorization
- Structured logging
- Redis cache
- More integration tests
- API documentation with XML comments
- CPF validation
- Pagination
- Filters and search 