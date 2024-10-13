# Oil Gas API

This API is part of a system to monitor sensor data from equipment in a large oil and gas plant. The system receives real-time sensor data, stores it in a database, and provides endpoints to retrieve sensor data, upload missing data via CSV, and calculate average sensor values.

The Front-end has a exclusive Repository which you can find at:

- [Front-end Repository](https://github.com/gvalluis-dev/oil-gas-dashboard) 

## Features

- Real-time Data Ingestion: Receives sensor data from various equipment in JSON format.
- CSV Upload: Allows uploading a CSV file with missing sensor data.
- Average Calculation: Provides average sensor values for the last 24 hours, 48 hours, 1 week, and 1 month.
- Swagger Documentation: API is fully documented with Swagger and can be tested directly via the Swagger UI.

## Architecture
#### 1. `/Controllers`
  - Input and output models (DTOs)
  - Middleware for error handling and authentication

#### 2. `/DTO`
 - Data Transfer Objects: used to transport data from layers and avoid external conflicts
  
#### 3. `/Data`
 - Layer responsible for accessing the database

#### 4. `/Models`
 - Objects for external facing (endpoints)

#### 5. `/Services`
 - Where the business rules happens

#### 6. `/Migrations`
 - Reserved for EntityFramework files

## Technologies Used
- .NET 8 (C#) for API backend
- Entity Framework Core for database interaction
- Swagger (Swashbuckle) for API documentation
- SQL Server or any other relational database

## Prerequisites

- .NET 8 SDK
- SQL Server (or any compatible database)
- Swagger UI for in-browser testing
- Visual Studio (for better use)

## Installation

**Clone the repository**:
   ```bash
   git clone https://github.com/gvalluis-dev/oil-gas-api
   cd oil-gas-api
   

**Install dependencies**:
```bash
dotnet restore
```

**Set up the database**:
- The example below is using the windows authentication and trusting the certificate
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OilGasDb;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

**Run the migrations - Set the database**:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**Run the API**:
```bash
dotnet run
```

**Explore the Swagger**:
```bash
Open a browser and navigate to https://localhost:7279/swagger/index.html to explore the API documentation and test the endpoints.
```
