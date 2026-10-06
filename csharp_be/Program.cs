using DotNetEnv;
using Npgsql;

Env.Load();

var databaseUrl = new NpgsqlConnectionStringBuilder
{
    Host = Environment.GetEnvironmentVariable("DB_HOST"),
    Port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"),
    Database = Environment.GetEnvironmentVariable("DB_NAME"),
    Username = Environment.GetEnvironmentVariable("DB_USER"),
    Password = Environment.GetEnvironmentVariable("DB_PASSWORD"),
    SslMode = Enum.Parse<SslMode>(Environment.GetEnvironmentVariable("DB_SSL_MODE") ?? "Disable")
}.ConnectionString;

Console.WriteLine($"DB_HOST={Environment.GetEnvironmentVariable("DB_HOST")}");
Console.WriteLine($"DB_PORT={Environment.GetEnvironmentVariable("DB_PORT")}");
Console.WriteLine($"DB_NAME={Environment.GetEnvironmentVariable("DB_NAME")}");
Console.WriteLine($"DB_USER={Environment.GetEnvironmentVariable("DB_USER")}");
Console.WriteLine($"DB_PASSWORD={Environment.GetEnvironmentVariable("DB_PASSWORD")}");
Console.WriteLine($"DB_SSL_MODE={Environment.GetEnvironmentVariable("DB_SSL_MODE")}");
Console.WriteLine($"DATABASE_URL={databaseUrl}");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors();

app.MapGet("/", () => "Hello from ASP.NET Core!");

app.MapGet("/user", async () =>
{
    var users = new List<string>();

    await using var connection = new NpgsqlConnection(databaseUrl);
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT name FROM users", connection);
    await using var reader = await command.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        users.Add(reader.GetString(0));
    }

    return new
    {
        source_code = "c#",
        users
    };
});

app.Run();
