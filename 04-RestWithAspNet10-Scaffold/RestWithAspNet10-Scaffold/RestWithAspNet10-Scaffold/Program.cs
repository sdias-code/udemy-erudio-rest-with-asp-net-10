using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<NumberService>();
builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
