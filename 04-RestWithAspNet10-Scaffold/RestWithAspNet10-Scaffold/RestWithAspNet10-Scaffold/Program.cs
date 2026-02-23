using Microsoft.AspNetCore.Mvc.Formatters;
using RestWithAspNet10_Scaffold.Configurations;
using RestWithAspNet10_Scaffold.Extensions;
using RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory;
using RestWithAspNet10_Scaffold.Files.Importers.Contract.Factory;
using RestWithAspNet10_Scaffold.Hypermedia.Filters;
using RestWithAspNet10_Scaffold.Repositories;
using RestWithAspNet10_Scaffold.Repositories.Implementation;
using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Services.Implementations;
using RestWithAspNet10_Scaffold.Services.Implementations.V1;
using RestWithAspNet10_Scaffold.Services.Implementations.V2;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

builder.Services.AddControllers( options =>
{
    options.Filters.Add<HypermediaFilter>();

    options.OutputFormatters.RemoveType<
        StringOutputFormatter>();

})
    .AddXmlSerializerFormatters()
    .AddContentNegotiation();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddHATEOASConfiguration();
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

builder.Services.AddTransient<CsvImporter>();
builder.Services.AddTransient<XlsxImporter>();
builder.Services.AddScoped<FileImporterFactory>();

builder.Services.AddTransient<XlsxExporter>();
builder.Services.AddTransient<CsvExporter>();
builder.Services.AddScoped<FileExporterFactory>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileServices, FileServicesImpl>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("DefaultPolicy");

app.UseAuthorization();

app.MapControllers();

app.UseHATEOASRoutes();

app.UseSwaggerConfig();

app.UseScalarConfig();

app.Run();
