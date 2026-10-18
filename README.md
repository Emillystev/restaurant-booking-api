# BookingSystem

A restaurant table reservation backend built with C# and ASP.NET Core. The project is being developed incrementally, starting with the domain model, database configuration, and application contracts.

**Status: work in progress.** The current version contains the project foundation. Business services are scaffolds, and booking endpoints are planned.

## Tech stack

| Technology | Current use |
| --- | --- |
| .NET 9 / C# | Target framework and language |
| ASP.NET Core Web API | API host and controller infrastructure |
| Entity Framework Core 9 | Database context and entity configurations |
| SQL Server | EF Core provider included for persistence |
| OpenAPI | JSON document exposed in the Development environment |
| FluentValidation | Packages included; validation integration in progress |
| Mapster | Package included for future DTO mapping |
| JWT Bearer authentication | Package included; authentication setup planned |
| Serilog | Package included; logging integration planned |

## Project structure

```text
BookingSystem/
├── BookingSystem.API/
│   ├── Controllers/
│   ├── Properties/
│   └── Program.cs
├── BookingSystem.Application/
│   ├── Auth/
│   ├── Common/Models/
│   ├── Reservations/
│   ├── Restaurants/
│   ├── Tables/
│   └── DependencyInjection.cs
├── BookingSystem.Domain/
│   ├── Common/
│   ├── Entities/
│   └── Enums/
├── BookingSystem.Infrastructure/
│   ├── Configurations/
│   └── AppDbContext.cs
└── BookingSystem.sln
```

| Project | Contents |
| --- | --- |
| `BookingSystem.Domain` | Entities, shared entity types, and enums |
| `BookingSystem.Application` | Service interfaces, service scaffolds, request/response DTOs, and pagination model |
| `BookingSystem.Infrastructure` | EF Core context, relationships, indexes, and constraints |
| `BookingSystem.API` | Application entry point, middleware, and OpenAPI setup |

## What is already in place

- Domain entities and their relationships.
- Customer and Admin roles; Indoor, Terrace, and VIP zones.
- Reservation statuses: Pending, Confirmed, Seated, Completed, Cancelled, and NoShow.
- EF Core configurations for all six entities.
- Restaurant soft deletion with a global query filter.
- Database constraints for positive table capacity, positive guest counts, and reservation end times after start times.
- Unique indexes for user emails, restaurant zones, table numbers within a zone, and restaurant closed dates.
- A row version property configured for table concurrency checks.
- UTC creation and update timestamps assigned by `SaveChangesAsync`.
- Separate `...Request` and `...Response` DTO classes, each in its own file.
- A shared `PagedResult<T>` model with pagination metadata.
- Service interfaces and implementation scaffolds for authentication, restaurants, tables, and reservations.
- An `AddApplication` registration helper, with integration still in progress.
- An API host with OpenAPI document generation in Development.

The EF Core rules above are defined in code; database creation and migrations are still planned. Service methods currently throw `NotImplementedException`.

## Domain model

| Entity | Purpose |
| --- | --- |
| `User` | Customer or administrator account |
| `Restaurant` | Contact details, opening hours, and reservation duration |
| `Zone` | A restaurant area, such as Indoor, Terrace, or VIP |
| `RestaurantTable` | A numbered table with capacity and active status within a zone |
| `Reservation` | A customer's table booking, time range, guest count, status, and special requests |
| `ClosedDate` | A date when a restaurant is closed, with an optional reason |

A restaurant has zones and closed dates. Each zone contains tables, and each reservation belongs to a table and a customer. Reservation timestamps use UTC.

## Request and response DTOs

DTOs are located in each application feature's `DTOs` directory.

| Feature | Requests | Responses |
| --- | --- | --- |
| Auth | `RegisterRequest`, `LoginRequest` | `AuthResponse` |
| Restaurants | `CreateRestaurantRequest`, `UpdateRestaurantRequest` | `RestaurantResponse` |
| Tables | `CreateTableRequest`, `UpdateTableRequest` | `TableResponse` |
| Reservations | `AvailabilityRequest`, `CreateReservationRequest`, `ReservationFilterRequest` | `AvailableTableResponse`, `ReservationResponse` |

The reservation service contract includes availability searches, booking creation and cancellation, paginated lists, and status changes. These operations still need business logic and API controllers.

## Run locally

Install the .NET 9 SDK. SQL Server will be needed when database integration is completed; the current API host can start without a database.

From the repository root, run the API project:

```bash
dotnet run --project BookingSystem.API --launch-profile http
```

The HTTP profile starts the application at `http://localhost:5101` in Development. Its OpenAPI JSON document is available at:

```text
http://localhost:5101/openapi/v1.json
```

The document currently has no application endpoints because controllers have not been implemented. There is no Swagger UI configured, and the root URL has no page.

An HTTPS profile is also defined at `https://localhost:7033`:

```bash
dotnet run --project BookingSystem.API --launch-profile https
```

The HTTPS profile requires a trusted ASP.NET Core development certificate.

### Development seed and test passwords

`DbSeeder` includes the following accounts for local Development testing:

| Role | Email | Test password |
| --- | --- | --- |
| Admin | `admin@booking.local` | `Admin123!` |
| Customer | `customer@booking.local` | `Customer123!` |

> **Warning:** these are public test credentials for Development seed data only. They must never be used in production.

In `BookingSystem.API/Program.cs`, `DbSeeder.SeedAsync(db)` is called inside `if (app.Environment.IsDevelopment())`, after database migrations are applied. This seed is not executed by application startup in Production.

### Full solution build

```bash
dotnet restore BookingSystem.sln
dotnet build BookingSystem.sln
```

**Known build issue:** the current full solution build reports `CS1061` for `AddValidatorsFromAssembly` in `BookingSystem.Application/DependencyInjection.cs`. A dependency injection package version conflict warning was also observed. Application registration and project dependencies are still being completed. The API project can be built and run independently.

### Database setup status

`AppDbContext` and SQL Server entity configurations are present. A database connection string, `DbContext` registration in the API host, and EF Core migrations have not been added yet. Database setup instructions will be added with that implementation.

## Next steps

- [ ] Complete application dependency registration and resolve the full solution build issue.
- [ ] Connect the API host to the application and infrastructure projects.
- [ ] Configure SQL Server and add the initial EF Core migration.
- [ ] Implement registration, login, password hashing, and JWT authentication.
- [ ] Implement restaurant, closed-date, and table management.
- [ ] Implement availability searches and reservation rules, including overlapping booking checks.
- [ ] Add API controllers and role-based authorization.
- [ ] Add request validators, DTO mapping, error handling, and structured logging.
- [ ] Add automated tests and API usage examples.
