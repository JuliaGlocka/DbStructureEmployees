# DbStructureEmployees

## [🇬🇧 English](#english-version) | [🇵🇱 Polski](#wersja-polska)

---

## English Version <a name="english-version"></a>

## 📌 Project Purpose & Scope

This is an **educational backend project** demonstrating enterprise .NET 
architecture patterns:

- Complex database relationships (hierarchical data modeling)
- Business logic implementation and validation
- Entity Framework Core ORM usage
- Unit testing with xUnit
- Docker containerization

**Intended for**: Developers learning .NET/ASP.NET Core concepts

### Known Limitations & Production Considerations

This project is intentionally scoped for learning. The following would be 
required for production deployment:

| Feature | Status | Notes |
|---------|--------|-------|
| Authentication | ❌ Not Implemented | Add ASP.NET Core Identity or OAuth2 |
| Authorization | ❌ Not Implemented | Add role-based access control (RBAC) |
| Logging | ❌ Not Implemented | Add Serilog for structured logging |
| Error Handling | ⚠️ Minimal | Add global exception middleware |
| Input Validation | ⚠️ Basic | Add FluentValidation framework |
| Async/Await | ❌ Not Implemented | Convert to async Task-based operations |
| HTTPS/TLS | ❌ Not Implemented | Configure certificate-based HTTPS |
| Health Checks | ❌ Not Implemented | Add /health endpoint for monitoring |
| API Documentation | ❌ Not Implemented | Add Swagger/OpenAPI |
| Performance Optimization | ⚠️ Basic | Add Redis caching for hierarchy queries |

### What I Learned Building This

**Technical**:
- Modeling hierarchical data with self-referential relationships
- Complex date range queries in LINQ
- Recursive algorithms for tree traversal
- EF Core Fluent API configuration
- xUnit testing patterns and edge cases

**Professional**:
- Clean architecture principles and layered design
- Importance of test coverage for business logic
- Docker containerization workflow
- Configuration management best practices

---

### Project Overview
This project is a .NET Core application designed to manage employee data and their vacation schedules. It uses Entity Framework Core with PostgreSQL as the database. Docker is used for containerization.

### Technologies
- .NET 8.0
- EF Core with PostgreSQL provider
- Docker (for containerization)
- Xunit for unit testing

### Project Structure
```bash
DbStructureEmployees/
│
├── Controllers/                                  // Controllers handling API or MVC requests
│ └── EmployeesController.cs                     // Controller managing employee-related endpoints
│
├── Data/                                         // EF Core configuration and DbContext
│ └── AppDbContext.cs                            // Main database context
│
├── DbStructureEmployees.Tests/                // Unit tests project
│ ├── DbStructureEmployees.Tests.csproj
│ ├── EmployeeStructureTest.cs             
│ ├── EmployeeTest.cs 
│ ├── bin/
│ └── obj/
│
├── Models/ 
│ └── Employee.cs 
│
├── Pages/                                       // Razor Pages (UI views)
│ ├── Error.cshtml                              // Error page
│ ├── Index.cshtml                             // Main/home page
│ ├── Privacy.cshtml                          // Privacy policy page
│ ├── Shared/                                // Shared layout and partial views
│ ├── _ViewImports.cshtml                   // Razor view imports
│ └── _ViewStart.cshtml                    // Razor view startup configuration
│
├── Properties/                             // Project properties and settings
│ └── launchSettings.json                  // Launch configuration for debugging
│
├── Services/                       // Business logic and service classes
│ └── EmployeeStructure.cs
│
├── wwwroot/                     // Static files (CSS, JS, images, libs)
│ ├── css/
│ ├── favicon.ico
│ ├── js/
│ └── lib/
│
├── Dockerfile                                  // Docker image build definition
├── Run-DockerContainer.ps1                    // PowerShell script to run Docker container
├── docker-compose.yml                        // Docker Compose configuration (if used)
├── appsettings.json                         // Main application configuration
├── appsettings.Development.json            // Development environment config
├── Program.cs                             // Application entry point (.NET Core)
├── README.md // Project documentation
├── DbStructureEmployees.csproj             // .NET project file
├── DbStructureEmployees.sln               // Visual Studio solution file
├── builddiag.txt                         // Optional diagnostic files
├── compileitems.txt
├── bin/                                // Compiled binaries output
└── obj/                               // Intermediate compilation files
```
 & Highlights
- **Models:** Employee, Vacation, VacationPackage, Team — core domain entities.
- **Services:** EmployeeQueries - contains queries and business logic for employees and vacations.
- **DbContext:** Manages database connection and entity mapping.
- **Key Methods:**
  - `GetEmployeesFromDotNetWithVacationIn2019()` — filters employees by team and vacations.
  - `CountFreeDaysForEmployee()` — calculates remaining vacation days.
  - `IfEmployeeCanRequestVacation()` — returns if employee can request vacation based on remaining days.

#### Code snippet - CountFreeDaysForEmployee:
```csharp
public static int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
{
    var year = DateTime.UtcNow.Year;
    var usedDays = vacations
        .Where(v => v.EmployeeId == employee.Id && v.DateStart.Year == year)
        .Sum(v => (v.DateEnd - v.DateStart).Days + 1);

    var freeDays = vacationPackage.TotalDays - usedDays;
    return freeDays > 0 ? freeDays : 0;
}
```

### Running the project

1. Clone repository:
```bash
git clone https://github.com/JuliaGlocka/DbStructureEmployees
cd DbStructureEmployees
```

2. Ensure PostgreSQL is running and connection string is updated in `appsettings.json`.

3. Apply migrations:
```bash
dotnet ef database update
```

4. Run project:
```bash
dotnet run
```

5. (Optional) Docker:
```bash
docker build -t dbstructureemployees .
docker run -p 5000:5000 dbstructureemployees
```

#### Personal Development Setup (Recommended)
If you're using Windows with Docker Desktop, here's a step-by-step approach that works well:

1. **Open Docker Desktop** (make sure it's running)

2. **Open Bash** and navigate to project directory

3. **Build Docker image**:
```bash
docker build -t your_dockerhub_username/your_image_name:your_tag .
```

4. **Open PowerShell** and navigate to project directory

5. **Run the container using PowerShell script**:
```powershell
.\Run-DockerContainer.ps1 -containerName "your_container_name" -imageName "your_dockerhub_username/your_image_name:tag" -portHost 5000 -portContainer 80
```

6. **Test the application**:
   - Open browser and go to: http://localhost:5000
   - Or test with curl in Bash: `curl http://localhost:5000`

### Verifying EF Core connection with PostgreSQL
- Use `dotnet ef migrations list` to check existing migrations.
- Use `dotnet ef database update` to apply migrations.
- Check logs for successful connection on startup.

### Tests
- Unit tests use Xunit.
- Test cases cover vacation request eligibility.
- Tests can be run with:
```bash
dotnet test
```

### Possible extensions
- Implement caching to reduce DB hits.
- Use DTOs to optimize data transfer.
- Extend lazy loading for related entities.

### Contact / Author
**Julia Głocka**
- Email: glockajulia@gmail.com
- GitHub: https://github.com/JuliaGlocka
---

## Wersja Polska <a name="wersja-polska"></a>

### Opis projektu
Projekt to aplikacja .NET Core do zarządzania pracownikami i ich urlopami. Korzysta z Entity Framework Core i bazy PostgreSQL. Projekt jest dockeryzowany.

### Technologie
- .NET 8.0
- EF Core z providerem PostgreSQL
- Docker (konteneryzacja)
- Xunit (testy jednostkowe)

### Struktura projektu i najważniejsze elementy
```bash
DbStructureEmployees/
│
├── Controllers/                                  // Kontrolery obsługujące żądania API lub MVC
│ └── EmployeesController.cs                     //  Kontroler zarządzający endpointami związanymi z pracownikami
│
├── Data/                                         //  Konfiguracja EF Core i DbContext
│ └── AppDbContext.cs                            //   Główny kontekst bazy danych
│
├── DbStructureEmployees.Tests/                // Projekt testów jednostkowych
│ ├── DbStructureEmployees.Tests.csproj
│ ├── EmployeeStructureTest.cs             
│ ├── EmployeeTest.cs 
│ ├── bin/
│ └── obj/
│
├── Models/                                         //  Modele danych
│ └── Employee.cs                                  //   Model reprezentujący pracownika
│
├── Pages/                                       // Razor Pages ( UI)
│ ├── Error.cshtml                              //  Strona błędu
│ ├── Index.cshtml                             //   Strona główna
│ ├── Privacy.cshtml                          //    Strona polityki prywatności
│ ├── Shared/                                //     Wspólny layout i widoki częściowe
│ ├── _ViewImports.cshtml                   //      Importy dla widoków Razor
│ └── _ViewStart.cshtml                    //       Konfiguracja startowa dla Razor Pages
│
├── Properties/                             // Właściwości projektu i ustawienia
│ └── launchSettings.json                  //  Konfiguracja uruchamiania do debugowania
│
├── Services/                       // Logika biznesowa i klasy usługowe
│ └── EmployeeStructure.cs         //  Klasa obsługująca strukturę pracowników
│
├── wwwroot/                     // Pliki statyczne (CSS, JS, obrazy, biblioteki)
│ ├── css/
│ ├── favicon.ico
│ ├── js/
│ └── lib/
│
├── Dockerfile                                  // Definicja budowania obrazu Docker
├── Run-DockerContainer.ps1                    // Skrypt PowerShell do uruchamiania kontenera Docker
├── docker-compose.yml                        // Konfiguracja Docker Compose (jeśli używana)
├── appsettings.json                         // Główna konfiguracja aplikacji
├── appsettings.Development.json            // Konfiguracja środowiska developerskiego
├── Program.cs                             // Punkt wejścia aplikacji (.NET Core)
├── README.md                             // Dokumentacja projektu
├── DbStructureEmployees.csproj             // Plik projektu .NET
├── DbStructureEmployees.sln               // Plik rozwiązania Visual Studio
├── builddiag.txt                         // Opcjonalne pliki diagnostyczne
├── compileitems.txt
├── bin/                                // Wyjściowe pliki binarne
└── obj/                               // Pliki pośrednie kompilacjis
```

- **Modele:** Employee, Vacation, VacationPackage, Team — podstawowe encje domenowe.
- **Serwisy:** EmployeeQueries — logika biznesowa i zapytania dotyczące pracowników i urlopów.
- **DbContext:** zarządza połączeniem z bazą i mapowaniem encji.
- **Kluczowe metody:**
  - `GetEmployeesFromDotNetWithVacationIn2019()` — zwraca pracowników z zespołu .NET z urlopami w 2019 r.
  - `CountFreeDaysForEmployee()` — oblicza pozostałą liczbę dni urlopu.
  - `IfEmployeeCanRequestVacation()` — sprawdza, czy pracownik może prosić o urlop.

#### Fragment kodu - CountFreeDaysForEmployee:
```csharp
public static int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
{
    var year = DateTime.UtcNow.Year;
    var usedDays = vacations
        .Where(v => v.EmployeeId == employee.Id && v.DateStart.Year == year)
        .Sum(v => (v.DateEnd - v.DateStart).Days + 1);

    var freeDays = vacationPackage.TotalDays - usedDays;
    return freeDays > 0 ? freeDays : 0;
}
```
### Uruchomienie projektu

1. Sklonuj repozytorium:
```bash
git clone https://github.com/JuliaGlocka/DbStructureEmployees
cd DbStructureEmployees
```

2. Upewnij się, że PostgreSQL działa i poprawnie skonfigurowano connection string w `appsettings.json`.

3. Wykonaj migracje:
```bash
dotnet ef database update
```

4. Uruchom projekt:
```bash
dotnet run
```

5. (Opcjonalnie) Docker:
```bash
docker build -t dbstructureemployees .
docker run -p 5000:5000 dbstructureemployees
```

#### Osobiste ustawienia rozwojowe (Zalecane)
Jeśli używasz Windows z Docker Desktop, oto krok po kroku podejście, które dobrze działa:

1. **Otwórz Docker Desktop** (upewnij się, że działa)

2. **Otwórz Bash** i przejdź do katalogu projektu

3. **Zbuduj obraz Docker**

4. **Otwórz PowerShell** i przejdź do katalogu projektu

5. **Uruchom kontener używając skryptu PowerShell**:
```powershell
.\Run-DockerContainer.ps1 -containerName "nazwa_twojego_kontenera" -imageName "nazwa_uzytkownika_dockerhub/nazwa_obrazu:tag" -portHost 5000 -portContainer 80
```

6. **Przetestuj aplikację**:
   - Otwórz przeglądarkę i idź do: http://localhost:5000
   - Lub przetestuj z curl w Bash: `curl http://localhost:5000`

### Weryfikacja połączenia EF Core z PostgreSQL
- `dotnet ef migrations list` pokazuje dostępne migracje.
- `dotnet ef database update` stosuje migracje.
- Logi startowe pokazują czy połączenie z bazą działa poprawnie.

### Testy
- Testy jednostkowe z Xunit.
- Testy sprawdzają możliwość zgłaszania urlopu.
- Uruchom testy poleceniem:
```bash
dotnet test
```

### Możliwe rozszerzenia
- Caching do ograniczenia zapytań do bazy.
- DTO do optymalizacji transferu danych.
- Lazy loading dla powiązanych encji.

### Kontakt / Autor
**Julia Głocka**
- Email: glockajulia@gmail.com
- GitHub: https://github.com/JuliaGlocka
