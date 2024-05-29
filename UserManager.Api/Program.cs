using Microsoft.EntityFrameworkCore;
using UserManager.Data.Data;
using UserManager.Data.Interfaces.Repositories.Users;
using UserManager.Data.Interfaces.Services;
using UserManager.Data.Interfaces.Services.Users;
using UserManager.Repositories;
using UserManager.Services;
using UserManager.Services.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
	.AddEnvironmentVariables();

builder.Services.AddControllers();

var configuration = builder.Configuration;

// Database
builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITimestampService, TimestampService>();

// Background service
builder.Services.AddHostedService<UserManager.Services.Background.BackgroundWorker>();

var app = builder.Build();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
