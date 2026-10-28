using income_expense_tracker.Data;
using income_expense_tracker.Exception;
using income_expense_tracker.Repositories;
using income_expense_tracker.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Missing ConnectionStrings:Postgres. Configure user secrets or environment variables.");

builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITranscationService, TransactionService>();

//exception haddler
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


var app = builder.Build();

if (app.Configuration.GetValue<bool>("ApplyMigrationsOnStartup"))
{
    app.Logger.LogInformation(
        "Starting automatic database migrations..."
    );

    using var scope = app.Services.CreateScope();

    var db = scope.ServiceProvider
        .GetRequiredService<TransactionDbContext>();

    var pendingMigrations =
        (await db.Database.GetPendingMigrationsAsync())
        .ToList();

    app.Logger.LogInformation(
        "Found {Count} pending database migrations.",
        pendingMigrations.Count
    );

    foreach (var migration in pendingMigrations)
    {
        app.Logger.LogInformation(
            "Pending migration: {Migration}",
            migration
        );
    }

    // Creates missing tables and applies pending
    // schema changes defined in EF migrations.
    await db.Database.MigrateAsync();

    app.Logger.LogInformation(
        "Database migrations completed successfully."
    );
}


app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();
//health check
app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "healthy",
        service = "pocketledger-api"
    });
}).AllowAnonymous();

app.Run();

public partial class Program { }