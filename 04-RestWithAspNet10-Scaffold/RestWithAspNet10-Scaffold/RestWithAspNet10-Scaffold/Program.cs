using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.IdentityModel.Tokens;
using RestWithAspNet10_Scaffold.Auth.Config;
using RestWithAspNet10_Scaffold.Auth.Contract;
using RestWithAspNet10_Scaffold.Auth.Tools;
using RestWithAspNet10_Scaffold.Configurations;
using RestWithAspNet10_Scaffold.Extensions;
using RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory;
using RestWithAspNet10_Scaffold.Files.Importers.Contract.Factory;
using RestWithAspNet10_Scaffold.Hypermedia.Filters;
using RestWithAspNet10_Scaffold.Mail;
using RestWithAspNet10_Scaffold.Repositories;
using RestWithAspNet10_Scaffold.Repositories.Implementation;
using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Services.Implementations;
using RestWithAspNet10_Scaffold.Services.Implementations.V1;
using RestWithAspNet10_Scaffold.Services.Implementations.V2;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;

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

builder.Services.ConfigureEmail(builder.Configuration);

builder.Services.AddSwaggerConfig();
builder.Services.AddOpenAPIConfig();
builder.Services.AddRouteConfiguration();
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<NumberService>();
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.Configure<TokenConfiguration>(
    builder.Configuration.GetSection("TokenConfiguration"));

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IPasswordHasher, SecurePasswordHasher>();
builder.Services.AddScoped<IUserAuthService, UserAuthServiceImpl>();
builder.Services.AddScoped<ILoginService, LoginServiceImpl>();

builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonServiceImpl>();
builder.Services.AddScoped<IPersonServiceV2, PersonServiceImplV2>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookServiceImpl>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<EmailSender>();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("DefaultPolicy");

app.MapControllers();

app.UseHATEOASRoutes();

app.UseSwaggerConfig();

app.UseScalarConfig();

app.Run();
