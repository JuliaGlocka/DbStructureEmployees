using DbStructureEmployees.Data;
using DbStructureEmployees.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog before building the app
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "logs/app-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting application...");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog to the host
    builder.Host.UseSerilog();

    // Set the listening port to 80 (for Docker compatibility)
#pragma warning disable S1075
    builder.WebHost.UseUrls("http://*:80");
#pragma warning restore S1075

    // Add services to the container
    builder.Services.AddRazorPages();

    // Register custom services for dependency injection
    builder.Services.AddScoped<EmployeeQueries>();
    builder.Services.AddScoped<EmployeeStructure>();

    // Register the DbContext with dependency injection
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in configuration. " +
                "Ensure it's configured in appsettings.json or environment variables.");
        }

        options.UseNpgsql(connectionString);
    });

    var app = builder.Build();

    Log.Information("Application built successfully. Configuring middleware...");

    // Configure the HTTP request pipeline
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
        Log.Information("Running in Production mode");
    }
    else
    {
        Log.Information("Running in Development mode");
    }

    // Enable serving static files from the wwwroot folder
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    // Map Razor Pages endpoints
    app.MapRazorPages();

    Log.Information("Application configured. Starting to listen on port 80...");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}