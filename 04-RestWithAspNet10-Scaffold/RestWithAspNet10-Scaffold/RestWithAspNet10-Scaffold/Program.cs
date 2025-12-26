using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Configurations;
using RestWithAspNet10_Scaffold.Extensions;
using RestWithAspNet10_Scaffold.Repositories;
using RestWithAspNet10_Scaffold.Repositories.Implementation;
using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddControllers();
builder.Services.AddSingleton<NumberService>();
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

builder.Services.ConfigureSqlServer(builder.Configuration);


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
