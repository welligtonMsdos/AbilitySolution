using Ability.Api.Interfaces;
using Ability.Api.Services;
using Ability.Domain.Interfaces;
using Ability.Infrastructure.Repositories;
using MongoDB.Driver;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("Mongo");

var dbName = builder.Configuration["DatabaseName"] ?? "AbilityDb";

builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

builder.Services.AddScoped<INoticiaRepository>(sp =>
    new NoticiaRepository(sp.GetRequiredService<IMongoClient>(), dbName));

builder.Services.AddScoped<INoticiaService, NoticiaService>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference(options =>
{
    options.WithTitle("Ability API - Documentação")
           .WithTheme(ScalarTheme.Moon)
           .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
