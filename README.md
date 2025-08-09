# DbStructureEmployees

Sample ASP.NET Core 8.0 application using Razor Pages, demonstrating basic web structure and containerization with Docker.

## Description

DbStructureEmployees is a .NET 8.0 web application built with Razor Pages. It serves as a minimal example of how to structure a web project, publish it, and run it inside a Docker container. The app exposes a simple homepage and privacy page, and is ready to be extended with database integration and API endpoints.

## Technologies Used

- .NET 8.0 SDK and ASP.NET Core Runtime
- Razor Pages
- Docker (multi-stage build)
- PowerShell (for container automation)
- PostgreSQL (optional, via Docker Compose)

## Project Structure

```
DbStructureEmployees/
  Pages/
    Index.cshtml
    Privacy.cshtml
  wwwroot/
  Program.cs
  DbStructureEmployees.csproj
  appsettings.json
  Dockerfile
  docker-compose.yml
  Run-DockerContainer.ps1
  README.md
```

## .gitignore

To avoid tracking unnecessary files like `bin`, `obj`, `.vs`, etc., use a `.gitignore` file.

> **Note:** If files were already committed before `.gitignore` was added, run:
> ```bash
> git rm -r --cached .
> git add .
> git commit -m "Clean tracked files now ignored by .gitignore"
> ```

## Docker Setup

### 1. Login to Docker Hub

```bash
docker login
```

### 2. Build the image

```bash
docker build -t YOUR_DOCKER_USERNAME/dbstructureemployees:latest .
```

### 3. Run the container

```bash
docker run -d -p 5000:80 --name mydbstructureemployees YOUR_DOCKER_USERNAME/dbstructureemployees:latest
```

### 4. Stop and remove the container

```bash
docker stop mydbstructureemployees
docker rm mydbstructureemployees
```

## Dockerfile (Multi-Stage Build)

### Build stage:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out
```

### Runtime stage:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "DbStructureEmployees.dll"]
```

## PowerShell Script: Run-DockerContainer.ps1

Automates stopping/removing existing container and starting a new one.

```powershell
param(
    [string]$containerName = "mydbstructureemployees",
    [string]$imageName = "YOUR_DOCKER_USERNAME/dbstructureemployees:latest",
    [int]$portHost = 5000,
    [int]$portContainer = 80,
    [string]$environment = "Production"
)

$existing = docker ps -a --filter "name=$containerName" --format "{{.ID}}"

if ($existing) {
    Write-Host "Stopping container $containerName ..."
    docker stop $containerName
    Write-Host "Removing container $containerName ..."
    docker rm $containerName
} else {
    Write-Host "Container $containerName does not exist. Proceeding..."
}

Write-Host "Running new container $containerName ..."
docker run -d -p "${portHost}:${portContainer}" --name $containerName -e ASPNETCORE_ENVIRONMENT=$environment $imageName
```

### Usage Examples:

**Default settings (port 5000):**

```powershell
.\Run-DockerContainer.ps1
```

**Custom host port (port 8080):**

```powershell
.\Run-DockerContainer.ps1 -portHost 8080 -portContainer 80
```

**Development environment:**

```powershell
.\Run-DockerContainer.ps1 -environment "Development"
```

**Port 80 (requires admin rights on Windows):**

```powershell
.\Run-DockerContainer.ps1 -portHost 80 -portContainer 80
```

> **Port Parameters:**
> - `portHost`: The port on your host machine (external access)
> - `portContainer`: The internal container port (usually 80 for ASP.NET Core)

## Docker Compose

For running with PostgreSQL database:

```yaml
version: '3.8'

services:
  web:
    build: .
    ports:
      - "8080:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=db;Database=employeesdb;Username=postgres;Password=demo123
    depends_on:
      - db

  db:
    image: postgres:15
    environment:
      - POSTGRES_DB=employeesdb
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=demo123
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

### Run with Docker Compose:

```bash
docker-compose up -d
```

## Access the App

**Default PowerShell script settings:**

Open http://localhost:5000

**Custom port example:**

If you run with `-portHost 8080`, open http://localhost:8080

**Docker Compose:**

Open http://localhost:8080

You should see the default Razor Pages homepage with links to "Home" and "Privacy".

## Configuration and Run Summary

### 1. Dockerfile
- Uses multi-stage build: SDK for building the app, runtime for running it.
- Sets the environment variable `ASPNETCORE_ENVIRONMENT=Production` with the option to override.
- Exposes port 80, the standard for ASP.NET Core apps.
- Updates packages in the runtime image for security patches.

### 2. docker-compose.yaml
- Defines two services: `web` (our application) and `db` (PostgreSQL).
- Maps the app port to 8080 locally.
- Includes an example connection string with a hardcoded password, noted as for recruitment/demo purposes only.
- Uses `depends_on` to ensure proper startup order.

### 3. Run-DockerContainer.ps1
- A PowerShell script for easy container management with parameters (name, ports, environment, connection string).
- Stops and removes any existing container before running a new one.
- Allows overriding connection string and environment variables via Docker run parameters.

### 4. appsettings.json
- Minimal configuration without sensitive data.
- Logging and allowed hosts settings.
- Placeholder for connection strings can be added.

### 5. Program.cs
- Loads configuration from JSON files depending on the environment.
- Sets up ASP.NET Core middleware and HTTP pipeline.
- Enables flexible configuration management without code changes.

### 6. Best Practices & Security
- Passwords in config files are only for demo/recruitment purposes.
- In production, secrets should be managed securely (e.g., secret manager, environment variables).
- Configuration and deployment are flexible and easy to modify, facilitating testing and production rollout.

---

*This setup demonstrates a good understanding of modern ASP.NET Core containerized applications, with a focus on configuration separation, security, and ease of deployment.*

### Port Configuration Summary

| **File/Location** | **Configuration** |
|---|---|
| **Program.cs** | `builder.WebHost.UseUrls("http://*:80");` |
| **Dockerfile** | `EXPOSE 80` |
| **docker-compose.yml** | `ports: - "8080:80"` |
| **Run-DockerContainer.ps1** | `docker run -p 8080:80 ...` |

## Environment Configuration

The application supports different environments through the `ASPNETCORE_ENVIRONMENT` variable:

- **Production** (default): Optimized settings, minimal logging
- **Development**: Detailed error pages, verbose logging
- **Staging**: Production-like with additional debugging

### Override Environment:

```powershell
.\Run-DockerContainer.ps1 -environment "Development"
```

## Important Notes

### Port 80 on Windows
- Running on port 80 requires administrator rights on Windows
- Use elevated PowerShell or Command Prompt
- Alternative: Use a different host port like 8080

### Container Port Information
- The app **always** listens on port 80 inside the container
- External host port is configurable via script or docker-compose
- Port mapping format: `hostPort:containerPort`

### Troubleshooting
- Check if the port is free: `netstat -an | findstr :5000`
- Ensure Docker is running: `docker version`
- View container logs: `docker logs mydbstructureemployees`

## Security Best Practices

- **Never use hardcoded passwords in production**
- Use Docker secrets or environment variables for sensitive data
- Regularly update base images for security patches
- Use least-privilege principles for database connections

## Author

**Name:** glockajulia  
**Date:** August 2025  
**Location:** Warszawa, Polska