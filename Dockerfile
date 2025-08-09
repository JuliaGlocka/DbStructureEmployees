# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application files and build the app
COPY . .
RUN dotnet publish -c Release -o out

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Update packages for security patches
RUN apt-get update && apt-get upgrade -y && apt-get clean && rm -rf /var/lib/apt/lists/*


WORKDIR /app
COPY --from=build /app/out .

# Expose the port your app runs on
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "DbStructureEmployees.dll"]
