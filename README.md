# SpeakAI

SpeakAI is a .NET 8 web application that provides APIs for speech and AI-related services, including payment integration, file uploads, and real-time communication via SignalR.

## Features

- **RESTful API** with ASP.NET Core Controllers
- **Swagger/OpenAPI** documentation
- **Cloudinary** integration for file uploads
- **PayOS** payment gateway integration
- **SignalR** for real-time chat
- **Serilog** for structured logging
- **CORS** support
- **Memory caching**
- **EF Core** with MySQL (Pomelo)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/)
- (Optional) [Docker](https://www.docker.com/) for containerized deployment

## Getting Started

### 1. Clone the Repository
-git clone <repository-url> cd SpeakAI
### 2. Configuration
Copy `appsettings.json` and update the following sections with your credentials:
### 3. Database Migration
-dotnet ef database update --project DAL
### 4. Run the Application
dotnet run --project SpeakAI
The API will be available at `https://localhost:8081` (or as configured).

### 5. API Documentation

Navigate to `https://localhost:8081/swagger` to view and test the API endpoints.

## Project Structure

- **SpeakAI/**: Main ASP.NET Core Web API project
- **BLL/**: Business logic layer
- **DAL/**: Data access layer (EF Core)
- **Common/**: Shared DTOs and configurations

## Key Packages

- `Swashbuckle.AspNetCore` - Swagger/OpenAPI
- `CloudinaryDotNet` - Cloudinary API
- `payOS` - Payment gateway
- `Microsoft.AspNetCore.SignalR` - Real-time communication
- `Pomelo.EntityFrameworkCore.MySql` - MySQL provider for EF Core
- `Serilog` - Logging

## Development

- Visual Studio 2022 or later is recommended.
- All projects target `.NET 8`.

## License

This project is licensed under the MIT License.

---

**Note:** Replace placeholder values in configuration files with your actual credentials before running the application.
