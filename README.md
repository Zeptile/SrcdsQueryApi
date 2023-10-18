Certainly! Here's an improved version of your README:

# Srcds Server Information Query Client

This client provides a straightforward way to interact with the Server Query API for the Source engine developed by Valve. You can learn more about the Source engine's Server Query API on the [Valve Developer Community](https://developer.valvesoftware.com/wiki/Server_queries) website.

## Development

### Requirements

Before you start working with this project, make sure you have the following prerequisites in place:

- **.NET 7 Framework**: Ensure you have .NET 7 installed.
- **Docker** (Optional): If you prefer running the application in a container, Docker is also supported.

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

These instructions should help you set up and run the Srcds Server Information Query Client in both local and containerized environments.
