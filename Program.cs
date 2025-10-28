using back_test_project.Data;
using back_test_project.Repositories;
using back_test_project.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// delete folder Migrations   
////dotnet ef migrations add Initial
////dotnet ef database update

////dotnet remove package Npgsql.EntityFrameworkCore.PostgreSQL
////dotnet add package Microsoft.EntityFrameworkCore.SqlServer
//builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

////dotnet remove package Microsoft.EntityFrameworkCore.SqlServer
////dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

//порт задавать только в облаке
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();


// ѕример минимального endpointТа
app.MapGet("/", () => Results.Text("OK")).WithName("Root");
// явный healthcheck Ч быстрый и всегда 200
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.Run();
