using DbStructureEmployees.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Set the listening port to 80 (for Docker compatibility)
#pragma warning disable S1075 // URIs should not be hardcoded
builder.WebHost.UseUrls("http://*:80");
#pragma warning restore S1075 // URIs should not be hardcoded

// Add services to the container.
builder.Services.AddRazorPages();
// Register the DbContext with dependency injection
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); //minimal hosting model

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    // See: https://aka.ms/aspnetcore-hsts
    app.UseHsts();
}

// Enable serving static files from the wwwroot folder
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map Razor Pages endpoints
app.MapRazorPages();

await app.RunAsync();