using back_test_project.Data;
using back_test_project.Repositories;
using back_test_project.Services;
using back_test_project.Validation.Books;
using back_test_project.Validation.Cats;
using back_test_project.Validation.Employees;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<BookCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BookUpdateDtoValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<CatCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CatUpdateDtoValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<EmployeeCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeUpdateDtoValidator>();


// ---------- DB connection (Render + local) ----------
string? rawConn =
    Environment.GetEnvironmentVariable("ConnectionStrings__Default")  // Render (как у Вас)
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")             // альтернатива
    ?? builder.Configuration.GetConnectionString("Default");          // локально

if (string.IsNullOrWhiteSpace(rawConn))
{
    throw new InvalidOperationException("No DB connection string found.");
}

// Преобразуем к валидной Npgsql key=value строке
static string BuildConn(string input)
{
    // URL format (Render.com)
    if (input.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        input.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri = new Uri(input);
        string[] ui = uri.UserInfo.Split(':', 2);
        string user = Uri.UnescapeDataString(ui[0]);
        string pass = ui.Length > 1 ? Uri.UnescapeDataString(ui[1]) : "";

        var b = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Database = uri.AbsolutePath.Trim('/'),
            Username = user,
            Password = pass,
            SslMode = SslMode.Require
        };
        return b.ToString();
    }

    // key=value format (local)
    if (input.Contains("="))
    {
        var b = new NpgsqlConnectionStringBuilder(input);
        if (b.SslMode == SslMode.Disable)
        {
            // Locally you can leave SslMode set to Disable; to avoid breaking anything we keep it as is.
            // If you need to force SSL, uncomment the lines below:
            // b.SslMode = SslMode.Require;
        }
        return b.ToString();
    }

    throw new ArgumentException("Unrecognized DB connection string format.");
}

string finalConn = BuildConn(rawConn);

// (optional) short log — without password
string masked = finalConn.Replace($"Password={new NpgsqlConnectionStringBuilder(finalConn).Password}", "Password=***");
Console.WriteLine($"[DB] Using connection: {masked}");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(finalConn));


builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<ICatRepository, CatRepository>();
builder.Services.AddScoped<ICatService, CatService>();

string? port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    //start only in Render.com - it is necessary there!

#pragma warning disable IDE0058 // Expression value is never used
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
#pragma warning restore IDE0058 // Expression value is never used
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
    bool canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { db = canConnect });
});

app.Run();
