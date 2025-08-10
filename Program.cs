var builder = WebApplication.CreateBuilder(args);

// 🔧 Set the listening port to 80 (for Docker compatibility)
builder.WebHost.UseUrls("http://*:80");

// Add services to the container.
builder.Services.AddRazorPages();
// TO DO: Add
// builder.Services.AddDbContext<AppDbContext>
// (options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.Run();