# DbStructureEmployees

## [🇬🇧 English](#english-version) | [🇵🇱 Polski](#wersja-polska)

---

## English Version <a name="english-version"></a>

### 📌 Project Overview

A production-ready ASP.NET Core 8.0 application demonstrating enterprise-grade patterns for employee management and organizational hierarchy. This project showcases modern .NET development practices including Entity Framework Core, structured logging, containerization, and comprehensive testing.

**Tech Stack:**
- ASP.NET Core 8.0 (Razor Pages)
- Entity Framework Core 9.0 with PostgreSQL
- Serilog for structured logging
- Docker containerization
- xUnit for unit testing

### 🎯 Key Features

#### Core Functionality
- **Hierarchical Employee Management**: Self-referential relationships for organizational structure
- **Vacation Management**: Track employee vacations with validation and remaining days calculation
- **Team Organization**: Group employees by teams with vacation analytics
- **Advanced Queries**: Complex LINQ queries for vacation reports and team analytics

#### Production-Ready Features
- ✅ **Global Exception Handling**: Centralized error handling with consistent response format
- ✅ **Structured Logging**: Serilog integration with console and file outputs
- ✅ **Health Checks**: Kubernetes-ready liveness and readiness endpoints
- ✅ **Input Validation**: Comprehensive validation with Data Annotations
- ✅ **Async Operations**: Full async/await pattern implementation
- ✅ **Docker Support**: Multi-stage Dockerfile with security updates
- ✅ **Configuration Management**: Environment-based settings with .env support

### 🏗️ Architecture & Design Patterns

**Layered Architecture:**
```
Presentation Layer (Razor Pages, Controllers)
    ↓
Service Layer (Business Logic)
    ↓
Data Access Layer (EF Core, DbContext)
    ↓
Database (PostgreSQL)
```

**Key Design Decisions:**
- **Repository Pattern**: DbContext acts as Unit of Work
- **Service Layer**: Business logic separated from controllers
- **Dependency Injection**: Built-in ASP.NET Core DI container
- **Middleware Pipeline**: Custom exception handling middleware

### 📂 Project Structure

```
DbStructureEmployees/
├── Controllers/              # MVC Controllers
│   └── EmployeesController.cs
├── Data/                     # EF Core Configuration
│   └── AppDbContext.cs       # Database context with Fluent API
├── Models/                   # Domain Entities
│   ├── Employee.cs           # Self-referential hierarchy
│   ├── Team.cs
│   ├── Vacation.cs
│   └── VacationPackage.cs
├── Services/                 # Business Logic Layer
│   ├── EmployeeQueries.cs    # Complex queries & calculations
│   └── EmployeeStructure.cs  # Hierarchy traversal
├── Middleware/
│   └── GlobalExceptionHandlerMiddleware.cs  # Centralized error handling
├── Pages/                    # Razor Pages UI
├── DbStructureEmployees.Tests/  # Unit Tests
│   ├── EmployeeTest.cs
│   ├── EmployeeStructureTest.cs
│   └── EmployeeVacationTest.cs
├── Dockerfile                # Multi-stage Docker build
├── docker-compose.yml        # Docker Compose orchestration
├── Program.cs                # Application entry point
└── appsettings.json          # Configuration
```

### 🚀 Getting Started

#### Prerequisites
- .NET 8.0 SDK
- PostgreSQL 15+
- Docker Desktop (optional, for containerization)

#### Local Development Setup

1. **Clone the repository**
```bash
git clone https://github.com/JuliaGlocka/DbStructureEmployees
cd DbStructureEmployees
```

2. **Configure PostgreSQL Connection**

Update `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=employees_db;User Id=postgres;Password=your_password;"
  }
}
```

3. **Apply Database Migrations**
```bash
dotnet ef database update
```

4. **Run the Application**
```bash
dotnet run
```

Application will be available at: `http://localhost:5115`

#### Docker Deployment

**Option 1: Docker Compose (Recommended)**
```bash
# Copy environment template
cp .env.development .env

# Edit .env with your settings
# Start both application and database
docker-compose up -d
```

**Option 2: Docker with PowerShell Script**
```powershell
# Build image
docker build -t dbstructureemployees:latest .

# Run container
.\Run-DockerContainer.ps1 -containerName "my-app" -imageName "dbstructureemployees:latest" -portHost 8080
```

**Option 3: Manual Docker Commands**
```bash
# Build
docker build -t dbstructureemployees .

# Run
docker run -d -p 8080:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  --name dbstructureemployees \
  dbstructureemployees
```

### 🧪 Testing

Run all unit tests:
```bash
dotnet test
```

Run tests with coverage:
```bash
dotnet test /p:CollectCoverage=true
```

**Test Coverage:**
- Employee model validation
- Vacation eligibility logic
- Hierarchy traversal algorithms
- Edge cases (null superiors, circular references)

### 📊 Key Code Examples

#### Calculating Remaining Vacation Days
```csharp
public static int CountFreeDaysForEmployee(
    Employee employee,
    List<Vacation> vacations,
    VacationPackage vacationPackage)
{
    var year = DateTime.UtcNow.Year;
    var usedDays = vacations
        .Where(v => v.EmployeeId == employee.Id && 
                    v.DateStart.Year == year)
        .Sum(v => (v.DateEnd - v.DateStart).Days + 1);
    
    var freeDays = vacationPackage.TotalDays - usedDays;
    return Math.Max(freeDays, 0);
}
```

#### Building Organizational Hierarchy
```csharp
public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
{
    var structure = new List<EmployeeStructure>();
    
    foreach (var emp in employees)
    {
        int level = 1;
        var currentSuperior = employees.FirstOrDefault(e => e.Id == emp.SuperiorId);
        
        while (currentSuperior != null)
        {
            structure.Add(new EmployeeStructure
            {
                EmployeeId = emp.Id,
                SuperiorId = currentSuperior.Id,
                SuperiorLevel = level
            });
            
            currentSuperior = employees.FirstOrDefault(e => 
                e.Id == currentSuperior.SuperiorId);
            level++;
        }
    }
    
    return structure;
}
```

### 🔍 API Endpoints

#### Health Checks
- `GET /health/live` - Liveness probe (Kubernetes-ready)
- `GET /health/ready` - Readiness probe (Kubernetes-ready)

#### Pages
- `/` - Home page
- `/Privacy` - Privacy policy
- `/Error` - Error handling page

### 📈 Performance Considerations

**Database Optimization:**
- Eager loading with `.Include()` to prevent N+1 queries
- Indexed foreign keys for hierarchical queries
- Connection pooling enabled by default

**Caching Opportunities (Not Implemented):**
- Redis cache for frequently accessed hierarchies
- Memory cache for vacation package lookups
- Distributed cache for multi-instance deployments

### 🔐 Security Features

- **Input Validation**: Data Annotations on all models
- **SQL Injection Protection**: Parameterized queries via EF Core
- **Error Information Disclosure**: Detailed errors only in Development mode
- **Security Headers**: Can be enhanced with middleware
- **Docker Security**: Multi-stage builds, non-root user recommended

### 📝 Configuration

**Environment Variables:**
```bash
ASPNETCORE_ENVIRONMENT=Development|Production
ConnectionStrings__DefaultConnection=<PostgreSQL connection string>
```

**appsettings.json:**
- Connection strings
- Serilog configuration (log levels, sinks)
- Allowed hosts

### 🛠️ Development Tools

**Useful Commands:**
```bash
# Watch mode (auto-reload on changes)
dotnet watch run

# List migrations
dotnet ef migrations list

# Create new migration
dotnet ef migrations add MigrationName

# Database update
dotnet ef database update

# Run specific test
dotnet test --filter FullyQualifiedName~EmployeeTest
```

### 🚧 Known Limitations

This project is designed for educational purposes. For production deployment, consider:

| Feature | Status | Recommendation |
|---------|--------|----------------|
| Authentication | ❌ | Implement ASP.NET Core Identity or OAuth2/OIDC |
| Authorization | ❌ | Add role-based or policy-based authorization |
| Rate Limiting | ❌ | Add middleware for API rate limiting |
| HTTPS/TLS | ⚠️ | Configure SSL certificates for production |
| API Versioning | ❌ | Implement versioning strategy |
| Caching | ❌ | Add Redis or memory cache |
| Monitoring | ⚠️ | Integrate Application Insights or Prometheus |
| Data Validation | ✅ | Implemented with Data Annotations |
| Error Handling | ✅ | Global middleware implemented |
| Logging | ✅ | Serilog structured logging |
| Health Checks | ✅ | Kubernetes-ready endpoints |

### 📚 What This Project Demonstrates

**Technical Skills:**
- Entity Framework Core with complex relationships
- Self-referential foreign keys for hierarchical data
- LINQ for complex queries (date ranges, aggregations)
- Async/await patterns throughout
- Structured logging with Serilog
- Global exception handling middleware
- Docker multi-stage builds
- Unit testing with xUnit

**Software Engineering Practices:**
- Clean Architecture principles
- Separation of concerns (layers)
- Dependency Injection
- SOLID principles
- Code documentation (XML comments)
- Configuration management
- Error handling strategies

### 🔄 Future Enhancements

**Short-term:**
- [ ] Add REST API endpoints (Controllers)
- [ ] Implement DTOs for API responses
- [ ] Add AutoMapper for object mapping
- [ ] Expand test coverage to >80%

**Medium-term:**
- [ ] Add authentication (ASP.NET Core Identity)
- [ ] Implement authorization with policies
- [ ] Add Swagger/OpenAPI documentation
- [ ] Implement CQRS pattern with MediatR

**Long-term:**
- [ ] Migrate to Clean Architecture structure
- [ ] Add event sourcing for audit trail
- [ ] Implement Redis caching
- [ ] Add SignalR for real-time notifications
- [ ] Containerize with Kubernetes manifests

### 🤝 Contributing

This is an educational project, but suggestions and improvements are welcome!

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### 📞 Contact

**Julia Głocka**
- Email: glockajulia@gmail.com
- GitHub: [@JuliaGlocka](https://github.com/JuliaGlocka)
- LinkedIn: [Connect with me](https://linkedin.com/in/julia-glocka)

### 📄 License

This project is open source and available under the MIT License.

---

## Wersja Polska <a name="wersja-polska"></a>

### 📌 Przegląd Projektu

Aplikacja ASP.NET Core 8.0 gotowa do produkcji, demonstrująca wzorce enterprise dla zarządzania pracownikami i strukturą organizacyjną. Projekt prezentuje nowoczesne praktyki programowania .NET, w tym Entity Framework Core, strukturalne logowanie, konteneryzację i kompleksowe testowanie.

**Stack Technologiczny:**
- ASP.NET Core 8.0 (Razor Pages)
- Entity Framework Core 9.0 z PostgreSQL
- Serilog do strukturalnego logowania
- Konteneryzacja Docker
- xUnit do testów jednostkowych

### 🎯 Kluczowe Funkcje

#### Główna Funkcjonalność
- **Hierarchiczne Zarządzanie Pracownikami**: Samo-referencyjne relacje dla struktury organizacyjnej
- **Zarządzanie Urlopami**: Śledzenie urlopów z walidacją i obliczaniem pozostałych dni
- **Organizacja Zespołów**: Grupowanie pracowników z analizą urlopów
- **Zaawansowane Zapytania**: Kompleksowe zapytania LINQ dla raportów urlopowych

#### Funkcje Gotowe do Produkcji
- ✅ **Globalna Obsługa Wyjątków**: Scentralizowana obsługa błędów
- ✅ **Strukturalne Logowanie**: Integracja Serilog
- ✅ **Health Checks**: Endpointy dla Kubernetes
- ✅ **Walidacja Danych**: Kompleksowa walidacja
- ✅ **Operacje Asynchroniczne**: Pełny wzorzec async/await
- ✅ **Wsparcie Docker**: Wieloetapowy Dockerfile
- ✅ **Zarządzanie Konfiguracją**: Ustawienia zależne od środowiska

### 🏗️ Architektura

**Architektura Warstwowa:**
```
Warstwa Prezentacji (Razor Pages, Controllers)
    ↓
Warstwa Usług (Logika Biznesowa)
    ↓
Warstwa Dostępu do Danych (EF Core, DbContext)
    ↓
Baza Danych (PostgreSQL)
```

### 🚀 Szybki Start

#### Wymagania
- .NET 8.0 SDK
- PostgreSQL 15+
- Docker Desktop (opcjonalnie)

#### Konfiguracja Lokalna

1. **Sklonuj repozytorium**
```bash
git clone https://github.com/JuliaGlocka/DbStructureEmployees
cd DbStructureEmployees
```

2. **Skonfiguruj Połączenie z PostgreSQL**

Zaktualizuj `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=employees_db;User Id=postgres;Password=twoje_haslo;"
  }
}
```

3. **Zastosuj Migracje**
```bash
dotnet ef database update
```

4. **Uruchom Aplikację**
```bash
dotnet run
```

Aplikacja dostępna pod: `http://localhost:5115`

#### Wdrożenie Docker

**Opcja 1: Docker Compose (Zalecane)**
```bash
# Skopiuj szablon środowiska
cp .env.development .env

# Edytuj .env z własnymi ustawieniami
# Uruchom aplikację i bazę danych
docker-compose up -d
```

**Opcja 2: PowerShell Script**
```powershell
# Zbuduj obraz
docker build -t dbstructureemployees:latest .

# Uruchom kontener
.\Run-DockerContainer.ps1 -containerName "moja-app" -imageName "dbstructureemployees:latest" -portHost 8080
```

### 🧪 Testowanie

Uruchom wszystkie testy:
```bash
dotnet test
```

**Pokrycie Testami:**
- Walidacja modelu Employee
- Logika uprawnień urlopowych
- Algorytmy przechodzenia hierarchii
- Przypadki brzegowe

### 📊 Przykłady Kodu

#### Obliczanie Pozostałych Dni Urlopu
```csharp
public static int CountFreeDaysForEmployee(
    Employee employee,
    List<Vacation> vacations,
    VacationPackage vacationPackage)
{
    var year = DateTime.UtcNow.Year;
    var usedDays = vacations
        .Where(v => v.EmployeeId == employee.Id && 
                    v.DateStart.Year == year)
        .Sum(v => (v.DateEnd - v.DateStart).Days + 1);
    
    var freeDays = vacationPackage.TotalDays - usedDays;
    return Math.Max(freeDays, 0);
}
```

### 🔍 Endpointy API

#### Health Checks
- `GET /health/live` - Sonda żywotności
- `GET /health/ready` - Sonda gotowości

### 🚧 Znane Ograniczenia

Projekt stworzony w celach edukacyjnych. Dla wdrożenia produkcyjnego rozważ:

| Funkcja | Status | Rekomendacja |
|---------|--------|--------------|
| Uwierzytelnianie | ❌ | Zaimplementuj ASP.NET Core Identity |
| Autoryzacja | ❌ | Dodaj kontrolę dostępu opartą na rolach |
| Limitowanie Zapytań | ❌ | Dodaj middleware rate limiting |
| HTTPS/TLS | ⚠️ | Skonfiguruj certyfikaty SSL |
| Cache | ❌ | Dodaj Redis lub memory cache |
| Monitoring | ⚠️ | Zintegruj Application Insights |
| Walidacja Danych | ✅ | Zaimplementowano |
| Obsługa Błędów | ✅ | Middleware zaimplementowany |
| Logowanie | ✅ | Serilog skonfigurowany |

### 📚 Co Demonstruje Ten Projekt

**Umiejętności Techniczne:**
- Entity Framework Core ze złożonymi relacjami
- Samo-referencyjne klucze obce dla danych hierarchicznych
- LINQ dla złożonych zapytań
- Wzorce async/await
- Strukturalne logowanie z Serilog
- Globalna obsługa wyjątków
- Wieloetapowe buildy Docker
- Testowanie jednostkowe z xUnit

**Praktyki Inżynierii Oprogramowania:**
- Zasady Clean Architecture
- Separacja odpowiedzialności
- Dependency Injection
- Zasady SOLID
- Dokumentacja kodu (komentarze XML)
- Zarządzanie konfiguracją
- Strategie obsługi błędów

### 🔄 Przyszłe Ulepszenia

**Krótkoterminowe:**
- [ ] Dodać endpointy REST API
- [ ] Zaimplementować DTO dla odpowiedzi API
- [ ] Dodać AutoMapper
- [ ] Rozszerzyć pokrycie testów do >80%

**Średnioterminowe:**
- [ ] Dodać uwierzytelnianie (Identity)
- [ ] Zaimplementować autoryzację z politykami
- [ ] Dodać dokumentację Swagger/OpenAPI
- [ ] Zaimplementować wzorzec CQRS z MediatR

**Długoterminowe:**
- [ ] Migracja do struktury Clean Architecture
- [ ] Dodać event sourcing dla śladu audytu
- [ ] Zaimplementować cache Redis
- [ ] Dodać SignalR dla powiadomień real-time

### 📞 Kontakt

**Julia Głocka**
- Email: glockajulia@gmail.com
- GitHub: [@JuliaGlocka](https://github.com/JuliaGlocka)
- LinkedIn: [Połącz się ze mną](https://linkedin.com/in/julia-glocka)

### 📄 Licencja

Projekt open source dostępny na licencji MIT.
