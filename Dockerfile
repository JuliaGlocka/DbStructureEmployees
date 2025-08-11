# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory inside the container
WORKDIR /app

# Copy solution and project files and restore dependencies for all projects
COPY DbStructureEmployees.sln .
COPY DbStructureEmployees.csproj .
COPY DbStructureEmployees.Tests/DbStructureEmployees.Tests.csproj ./DbStructureEmployees.Tests/
RUN dotnet restore DbStructureEmployees.sln

# Copy the rest of the application files to the container
COPY . .

# Remove previous build artifacts (bin and obj folders) to avoid duplicate attribute errors
RUN find . -type d -name "obj" -exec rm -rf {} +
RUN find . -type d -name "bin" -exec rm -rf {} +

# Build and publish the application in Release configuration to the 'out' folder
RUN dotnet publish DbStructureEmployees.csproj -c Release -o out

# Use the official ASP.NET Core runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Apply system updates and clean up to reduce image size and improve security
RUN apt-get update && apt-get upgrade -y && apt-get clean && rm -rf /var/lib/apt/lists/*

# Set working directory for the runtime image
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app/out .

# Set environment variable, can be overridden during docker run or in docker-compose
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port 80 for incoming HTTP requests
EXPOSE 80

# Define the command to run the application
ENTRYPOINT ["dotnet", "DbStructureEmployees.dll"]
