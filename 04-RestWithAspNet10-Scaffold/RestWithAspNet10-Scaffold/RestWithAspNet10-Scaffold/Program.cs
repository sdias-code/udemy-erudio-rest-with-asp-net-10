using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Extensions;
using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<NumberService>();
builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();
builder.Services.ConfigureSqlServer(builder.Configuration);


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
