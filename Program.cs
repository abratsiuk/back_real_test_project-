using back_test_project.Data;
using back_test_project.Repositories;
using back_test_project.Services;
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


//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// ---------- DB connection (Render + local) ----------
string? rawConn =
    Environment.GetEnvironmentVariable("ConnectionStrings__Default")  // Render (как у Вас)
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")             // альтернатива
    ?? builder.Configuration.GetConnectionString("Default");          // локально

if (string.IsNullOrWhiteSpace(rawConn))
    throw new InvalidOperationException("No DB connection string found.");

// Преобразуем к валидной Npgsql key=value строке
string BuildConn(string input)
{
    // URL-формат? (postgres://... или postgresql://...)
    if (input.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
        input.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
    {
        var uri = new Uri(input);
        var ui = uri.UserInfo.Split(':', 2);
        var user = Uri.UnescapeDataString(ui[0]);
        var pass = ui.Length > 1 ? Uri.UnescapeDataString(ui[1]) : "";

        var b = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Database = uri.AbsolutePath.Trim('/'),
            Username = user,
            Password = pass,
            SslMode = SslMode.Require,
            TrustServerCertificate = true
        };
        return b.ToString();
    }

    // key=value формат (локально)
    if (input.Contains("="))
    {
        var b = new NpgsqlConnectionStringBuilder(input);
        if (b.SslMode == SslMode.Disable)
        {
            // локально можно оставить Disable; на всякий случай не ломаем,
            // но если нужно принудить SSL, раскомментируйте:
            // b.SslMode = SslMode.Require;
            // b.TrustServerCertificate = true;
        }
        return b.ToString();
    }

    throw new ArgumentException("Unrecognized DB connection string format.");
}

var finalConn = BuildConn(rawConn);

// (опционально) короткий лог — без пароля
var masked = finalConn.Replace($"Password={new NpgsqlConnectionStringBuilder(finalConn).Password}", "Password=***");
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
