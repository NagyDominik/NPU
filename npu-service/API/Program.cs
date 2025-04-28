using Application.Services;
using Infrastructure.Messaging;
using Infrastructure.SQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddApiVersioning(cfg => { cfg.ReportApiVersions = true; })
    .AddMvc()
    .AddApiExplorer(cfg => { cfg.SubstituteApiVersionInUrl = true; });

// Validate required configuration
ValidateRequiredConfigurations(builder.Configuration);

// Configure PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NpuDBContext>(options =>
    options.UseNpgsql(connectionString));

// Configure RabbitMQ service
builder.Services.AddSingleton<IRabbitMQService>(provider =>
{
    var logger = provider.GetRequiredService<ILogger<RabbitMQService>>();
    var hostName = builder.Configuration["RabbitMQ:HostName"];
    var userName = builder.Configuration["RabbitMQ:UserName"];
    var password = builder.Configuration["RabbitMQ:Password"];

    // All values should be validated at this point so null-suppression is fine for now
    return new RabbitMQService(hostName!, userName!, password!, logger);
});

// Register repositories and services
builder.Services.AddScoped<INpuRepository, NpuRepository>();
builder.Services.AddScoped<INpuService, NpuService>();

var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NpuDBContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Validate that all required configuration values are present
// FIXME: Should be typed classes instead for better maintainability
void ValidateRequiredConfigurations(IConfiguration configuration)
{
    // Validate database connection string
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Database connection string 'DefaultConnection' is missing in configuration.");
    }

    // Validate RabbitMQ configurations
    var rabbitMQHostName = configuration["RabbitMQ:HostName"];
    var rabbitMQUserName = configuration["RabbitMQ:UserName"];
    var rabbitMQPassword = configuration["RabbitMQ:Password"];

    if (string.IsNullOrEmpty(rabbitMQHostName))
    {
        throw new InvalidOperationException("RabbitMQ:HostName is missing in configuration.");
    }

    if (string.IsNullOrEmpty(rabbitMQUserName))
    {
        throw new InvalidOperationException("RabbitMQ:UserName is missing in configuration.");
    }

    if (string.IsNullOrEmpty(rabbitMQPassword))
    {
        throw new InvalidOperationException("RabbitMQ:Password is missing in configuration.");
    }
}
