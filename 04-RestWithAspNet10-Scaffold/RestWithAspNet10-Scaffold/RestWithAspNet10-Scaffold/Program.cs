using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Configurations;
using RestWithAspNet10_Scaffold.Extensions;
using RestWithAspNet10_Scaffold.Repositories;
using RestWithAspNet10_Scaffold.Repositories.Implementation;
using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Services.Implementations;
using RestWithAspNet10_Scaffold.Services.Implementations.V1;
using RestWithAspNet10_Scaffold.Services.Implementations.V2;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddControllers()
    .AddContentNegotiation();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddOpenAPIConfig();
builder.Services.AddRouteConfiguration();
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<NumberService>();
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonServiceImpl>();
builder.Services.AddScoped<IPersonServiceV2, PersonServiceImplV2>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookServiceImpl>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCorsConfiguration(builder.Configuration);

app.UseAuthorization();

app.MapControllers();

app.UseSwaggerConfig();

app.UseScalarConfig();

app.Run();
