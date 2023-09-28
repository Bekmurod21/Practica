using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Practic.Data.DbContexts;
using Practic.Data.IRepositories;
using Practic.Data.Repositories;
using Practic.Extensions;
using Practic.Middlewares;
using Practic.Models;
using Practic.Service.Interfaces.Users;
using Practic.Service.Mappers;
using Practic.Service.Service.Users;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.Configure<ForwardedHeadersOptions>(options =>
//{
//    options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
//});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(builder.Configuration
                .GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(
                                        new ConfigureApiUrlName()));
});
builder.Services.AddControllersWithViews()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .Enrich
    .FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
//});

app.ApplyMigrations();
app.InitAccessor();


if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
