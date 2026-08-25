using System.Text.Json.Serialization;
using Argos.Api.Exceptions;
using Argos.Api.Extensions;
using Argos.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    // Enums saem como string UPPER_SNAKE no JSON (ex.: "ALTO", "EM_ANALISE") — contrato do app.
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSwagger();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddDbContext(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArgosContext>();

    // 1. Cria as tabelas / aplica as migrations
    db.Database.Migrate();

    // 2. Executa a injeção de dados da sua seed
    // O método SeedAsync pede um IServiceProvider, que temos no scope!
    await ArgosSeeder.SeedAsync(scope.ServiceProvider);
}

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
