using back_test_project.Data;
using back_test_project.Repositories;
using back_test_project.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowWeb", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});


//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
// Read connection from env (DATABASE_URL or ConnectionStrings__Default), normalize SSL for Render
string? conn =
    Environment.GetEnvironmentVariable("ConnectionStrings__Default")
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("Default");

if (conn is null)
    throw new InvalidOperationException("No DB connection string found (Default / DATABASE_URL).");

// If it's in postgres:// format (Render), convert to Npgsql standard and force SSL
if (conn.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
{
    // On Render, the PostgreSQL connection string usually arrives as: postgres://user:pass@host:port/db
    var uri = new Uri(conn);
    var userInfo = uri.UserInfo.Split(':', 2);
    var npg = new Npgsql.NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo.Length > 1 ? userInfo[1] : "",
        Database = uri.AbsolutePath.Trim('/'),
        SslMode = Npgsql.SslMode.Require,
        TrustServerCertificate = true
    };
    conn = npg.ToString();
}
else
{
    // Ensure SSL for plain Npgsql string on Render
    var b = new Npgsql.NpgsqlConnectionStringBuilder(conn);
    if (b.SslMode == Npgsql.SslMode.Disable)
    {
        b.SslMode = Npgsql.SslMode.Require;
        b.TrustServerCertificate = true;
    }
    conn = b.ToString();
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));



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
app.MapGet("/health", () => Results.Ok(new { status = "healthy 29102025 1700" }));

//check that database is updated
app.MapGet("/db-check", async (AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { db = canConnect });
});

app.Run();
