using System.Reflection;
using GestionClinicaNutricional.Infrastructure;
//using GestionClinicaNutricional.Infrastructure.DependencyInjection;
using GestionClinicaNutricionalService;

var builder = WebApplication.CreateBuilder(args);

// Disable service validation to prevent crashes on startup if some optional services are missing
builder.Host.UseDefaultServiceProvider(options => 
{
    // options.ValidateScopes = false;
    // options.ValidateOnBuild = false;
});



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddControllers();//defines the services that are required by the MVC framework
//builder.Services.AddOpenApiDocument();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApiDocument(config =>
{
    config.SchemaSettings.FlattenInheritanceHierarchy = true;
    //config.SchemaSettings.SchemaProcessors.Add(new ExcludeHabitoAlimenticioPropertySchemaProcessor());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
//}

app.UseHttpsRedirection();

app.MapControllers();// defines routes that will allow controllers to handle requests

app.Run();//start listening http request to asp.net core server

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };
//
// app.MapGet(
//         "/weatherforecast",
//         () =>
//         {
//             var forecast = Enumerable.Range(1, 5).Select(
//                     index =>
//                         new WeatherForecast(
//                             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                             Random.Shared.Next(-20, 55),
//                             summaries[Random.Shared.Next(summaries.Length)]))
//                 .ToArray();
//             return forecast;
//         })
//     .WithName("GetWeatherForecast");
//
// app.Run();
//
// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
// }