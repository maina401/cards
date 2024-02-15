# Cards API

This is a simple API for managing cards. It's built with .NET Core and uses Entity Framework Core for data access.

## Features

- User registration and authentication
- CRUD operations for cards
- Pagination, sorting, and filtering for retrieving cards
- Role-based authorization

## Endpoints

- `POST /auth/register`: Register a new user
- `POST /auth/login`: Authenticate a user and get a JWT
- `GET /card`: Get all cards with optional pagination, sorting, and filtering
- `GET /card/{id}`: Get a specific card by ID
- `POST /card`: Create a new card
- `PUT /card`: Update an existing card
- `DELETE /card/{id}`: Delete a card

## Setup

1. Clone the repository
2. Run `dotnet restore` to restore the NuGet packages
3. Run `dotnet ef database update` to apply the database migrations
4. Update your appsettings.json file with your database connection string or environment variable SA_PASSWORD if running with docker
5. Run `dotnet run` to start the application
6. visit `https://localhost:5001/swagger` to view the API documentation


## Seeding the Database

The database is seeded with an admin user and a member user at startup. The credentials for these users are:

- Admin: `admin@example.com` / `Admin123!`
- Member: `member@example.com` / `Member123!`

## Docker

A `Dockerfile` and `docker-compose.yml` file are included for running the application with Docker. To start the application with Docker, run `docker-compose up`.


## License

[MIT](https://choosealicense.com/licenses/mit/)