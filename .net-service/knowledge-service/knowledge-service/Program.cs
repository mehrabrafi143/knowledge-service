using knowledge_service.Data;
using knowledge_service.Models;
using knowledge_service.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// SQL Server Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Application Services
builder.Services.AddScoped<IKnowledgeService, KnowledgeService>();

// CORS for FastAPI integration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// ✅ CORRECT: Use EF Core Migrations to create/update database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // This will apply any pending migrations and create the database if it doesn't exist
        context.Database.Migrate();

        // Optional: Seed initial data
        await SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}

app.Run();

// Seed method for initial data
static async Task SeedDataAsync(ApplicationDbContext context)
{
    // Only seed if database is empty
    if (!await context.KnowledgeEntries.AnyAsync())
    {
        var entries = new[]
        {
            new KnowledgeEntry
            {
                Title = "Machine Maintenance",
                Description = "Regular maintenance procedures for industrial machines",
                Tags = new List<string> { "maintenance", "machines", "industrial" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new KnowledgeEntry
            {
                Title = "Safety Protocols",
                Description = "Safety guidelines for manufacturing floor",
                Tags = new List<string> { "safety", "protocols", "manufacturing" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new KnowledgeEntry
            {
                Title = "Quality Control",
                Description = "Quality assurance procedures and checklists",
                Tags = new List<string> { "quality", "control", "assurance" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new KnowledgeEntry
            {
                Title = "Troubleshooting Guide",
                Description = "Common issues and solutions for equipment",
                Tags = new List<string> { "troubleshooting", "guide", "equipment" },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await context.KnowledgeEntries.AddRangeAsync(entries);
        await context.SaveChangesAsync();
    }
}