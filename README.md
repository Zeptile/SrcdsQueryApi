# Server Query API - Quick .NET Implementation

I quickly wrote an implementation of the Valve Server Query API as it was needed for my internal server operations. There is a great chance I split the pure code into a library with nuget builds.

## Development


### Requirements

Before you start working with this project, make sure you have the following prerequisites in place:

- **.NET 7 Framework**
- **Docker** 

### Running Locally with Dotnet CLI

To run the application locally using the Dotnet CLI, follow these steps:

1. Set the environment variables for the ASP.NET Core application:

   ```sh
   export ASPNETCORE_URLS="http://0.0.0.0:5295"
   export ASPNETCORE_ENVIRONMENT="Development"
   ```

2. Run the application:
   ```sh
   dotnet run .
   ```

### Running Locally with Docker

If you'd like to run the application within a Docker container, use the following commands:

1. Build and start the Docker container:
   ```sh
   docker-compose up -d --build
   ```

