using back_test_project.Data;
using back_test_project.Repositories;
using back_test_project.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policy => policy
        .WithOrigins(
            "https://abratsiuk.github.io",
            "https://back-test-api.onrender.com",
            "http://localhost:4200",
            "http://0.0.0.0:10000",
            "http://0.0.0.0:1000"
        )
        .AllowAnyHeader()
        .AllowAnyMethod());
});



builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();


var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    //start only in Render.com
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

var app = builder.Build();

app.UseCors("AllowWeb");

// apply EF migrations automatically on container startup
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        logger.LogInformation("[INFO] Applying EF Core migrations...");
        db.Database.Migrate();
        logger.LogInformation("[INFO] Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[ERROR] Failed to apply database migrations.");
    }
}

//swagger for production in Rendewr.com
app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();


app.MapGet("/", () => Results.Text("OK")).WithName("Root");

//the fastest check API:
app.MapGet("/health", () => Results.Ok(new { status = "healthy 29102025 1638" }));

//check that database is updated
app.MapGet("/db-check", async (AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { db = canConnect });
});

app.MapMethods("{*path}", new[] { "OPTIONS" }, () => Results.Ok());

app.Run();
