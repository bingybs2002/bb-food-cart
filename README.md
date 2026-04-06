# BB Food Cart

BB Food Cart is a fullstack application for the imaginary failure Asian Computer student that didn't pass his CMPSC 464 class and can't graduate, and started to do a food cart on the nearby Gym.

### Backend features:
- JWT-based account registration and login
- Role-based access for `User` and `Admin`
- Menu management endpoints
- Customer shopping cart endpoints
- A gacha-style rewards API


## Stack

- .NET 10
- ASP.NET Core Minimal APIs and Controllers
- Entity Framework Core
- PostgreSQL
- ASP.NET Core Identity
- Swagger for local API exploration in development

## Project Layout

- [README.md](/Users/bingy/Projects/bb-food-cart/README.md): project overview and setup
- [Backend](/Users/bingy/Projects/bb-food-cart/Backend): backend API project
- [Backend/Program.cs](/Users/bingy/Projects/bb-food-cart/Backend/Program.cs): app startup, auth, Swagger, endpoint wiring
- [Backend/Data/AppDbContext.cs](/Users/bingy/Projects/bb-food-cart/Backend/Data/AppDbContext.cs): EF Core database context
- [Backend/EndPoints](/Users/bingy/Projects/bb-food-cart/Backend/EndPoints): account, cart, menu, and utility endpoints
- [Backend/Models](/Users/bingy/Projects/bb-food-cart/Backend/Models): domain models
- [Backend/Migrations](/Users/bingy/Projects/bb-food-cart/Backend/Migrations): EF Core migrations
- [Backend/Testing](/Users/bingy/Projects/bb-food-cart/Backend/Testing): example request payloads and sample data

## Prerequisites

- .NET SDK 10.x
- PostgreSQL running locally

## Local Configuration

The backend reads its connection string and JWT settings from [Backend/appsettings.json](/Users/bingy/Projects/bb-food-cart/Backend/appsettings.json).

Current local defaults:

```json
"ConnectionStrings": {
  "Connection": "Host=localhost; Database=bb-food; Username=postgres;Password=bing"
}
```

Before running locally, update this connection string to match your PostgreSQL setup if needed.

The project also includes a development JWT configuration in `appsettings.json`. That is fine for local work, but it should be replaced with environment-specific secrets before any shared or production deployment.

## Running The API

From the repo root:

```powershell
cd Backend
dotnet restore
dotnet ef database update
dotnet run
```

If `dotnet ef` is not installed:

```powershell
dotnet tool install --global dotnet-ef
```

The development launch profile is configured for:

- `https://localhost:63196`
- `http://localhost:63197`

Swagger is enabled only in development and is available at:

- `https://localhost:63196/swagger`
- `http://localhost:63197/swagger`

## Seeded Roles And Admin

On startup, the API ensures the `Admin` and `User` roles exist.

It also seeds a default admin account in [Backend/Models/Account/Admin.cs](/Users/bingy/Projects/bb-food-cart/Backend/Models/Account/Admin.cs):

- Phone number / username: `9179`
- Password: `9179`

This is convenient for local testing, but it is not safe for any environment beyond local development.

## Authentication Flow

The account endpoints are registered in [Backend/EndPoints/Account/AccountEndpoints.cs](/Users/bingy/Projects/bb-food-cart/Backend/EndPoints/Account/AccountEndpoints.cs).

Main auth routes:

- `POST /account/register`
- `POST /account/login`
- `POST /account/refresh`
- `POST /account/forgot-password`
- `POST /account/reset-password`
- `GET /account/me`
- `POST /account/logout`
- `POST /account/change-password`
- `POST /account/is-admin`
- `POST /account/promote-to-admin`

Protected endpoints use bearer authentication:

```http
Authorization: Bearer <jwt-token>
```

## Main API Areas

### Account

Register a customer:

```json
{
  "phoneNumber": "1234567833",
  "password": "Password33",
  "cosignee": "User33",
  "allergies": 2,
  "streetAddress": "33 Main St",
  "city": "City",
  "state": "State",
  "zipCode": "10033"
}
```

Login:

```json
{
  "phoneNumber": "1234567833",
  "password": "Password33"
}
```

More sample payloads are in:

- [Backend/Testing/Register.txt](/Users/bingy/Projects/bb-food-cart/Backend/Testing/Register.txt)
- [Backend/Testing/Login.txt](/Users/bingy/Projects/bb-food-cart/Backend/Testing/Login.txt)

### Menu

Menu endpoints are implemented in [Backend/EndPoints/ShoppingCart/Cart/AdminItem.cs](/Users/bingy/Projects/bb-food-cart/Backend/EndPoints/ShoppingCart/Cart/AdminItem.cs).

- `POST /itemAdmin/Create` for bulk menu creation, admin only
- `GET /itemAdmin/Read` to read all menu items
- `PUT /itemAdmin/update/{Name}` to update a menu item, admin only
- `DELETE /itemAdmin/delete/{Name}` to delete a menu item, admin only

Sample menu data is in [Backend/Testing/Menu/FoodItems.txt](/Users/bingy/Projects/bb-food-cart/Backend/Testing/Menu/FoodItems.txt).

### Shopping Cart

Customer cart endpoints are implemented in [Backend/EndPoints/ShoppingCart/Cart/ShoppingCart.cs](/Users/bingy/Projects/bb-food-cart/Backend/EndPoints/ShoppingCart/Cart/ShoppingCart.cs).

- `GET /Cart/myCart`
- `POST /Cart/AddToCart`
- `POST /Cart/ChangeItemQuantity`
- `DELETE /Cart/DeleteItem`

Admin cart lookup is implemented in [Backend/EndPoints/ShoppingCart/Cart/AdminCart.cs](/Users/bingy/Projects/bb-food-cart/Backend/EndPoints/ShoppingCart/Cart/AdminCart.cs).

- `GET /cartAdmin/{phoneNumber}`

### Gacha Rewards

Gacha endpoints are implemented in [Backend/Models/Gacha/Gacha.cs](/Users/bingy/Projects/bb-food-cart/Backend/Models/Gacha/Gacha.cs).

- `POST /Gacha/insert`, admin only
- `DELETE /Gacha/delete/{id}`, admin only
- `DELETE /Gacha/delete-all`, admin only
- `GET /Gacha/pick`
- `GET /Gacha/all-items`

Sample gacha items are in:

- [Backend/Testing/Gacha/GachaItems.txt](/Users/bingy/Projects/bb-food-cart/Backend/Testing/Gacha/GachaItems.txt)
- [Backend/Testing/Gacha/GachaReadMe.txt](/Users/bingy/Projects/bb-food-cart/Backend/Testing/Gacha/GachaReadMe.txt)

## Notes And Gotchas

- The calories utility endpoint exists in [Backend/EndPoints/Calories/EndpointCalories.cs](/Users/bingy/Projects/bb-food-cart/Backend/EndPoints/Calories/EndpointCalories.cs), but it is not currently mapped in [Backend/Program.cs](/Users/bingy/Projects/bb-food-cart/Backend/Program.cs), so it is not active at runtime.
- The `frontend` directory is currently empty, so this repo behaves as a backend-only project today.
- Some text files in [Backend/Testing](/Users/bingy/Projects/bb-food-cart/Backend/Testing) describe older endpoint names. The README above reflects the routes currently wired in code.