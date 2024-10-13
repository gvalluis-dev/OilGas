# Oil Gas API

This API is part of a system to monitor sensor data from equipment in a large oil and gas plant. The system receives real-time sensor data, stores it in a database, and provides endpoints to retrieve sensor data, upload missing data via CSV, and calculate average sensor values.

The Front-end has a exclusive Repository which you can find at:

- [Front-end Repository](https://github.com/gvalluis-dev/oil-gas-dashboard) 

## Features

- Real-time Data Ingestion: Receives sensor data from various equipment in JSON format.
- CSV Upload: Allows uploading a CSV file with missing sensor data.
- Average Calculation: Provides average sensor values for the last 24 hours, 48 hours, 1 week, and 1 month.
- Swagger Documentation: API is fully documented with Swagger and can be tested directly via the Swagger UI.

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

- dotnet restore
