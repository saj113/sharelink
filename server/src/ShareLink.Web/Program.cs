using System.Text.Json.Serialization;
using ShareLink.Application;
using ShareLink.Dal;
using ShareLink.Migrations;
using ShareLink.Identity;
using ShareLink.Migrations.Initializers;
using ShareLink.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        var enumConverter = new JsonStringEnumConverter();
        opts.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder
    .Services
    .AddSwaggerGen()
    .AddLogging()
    .AddApplicationServices(builder.Configuration)
    .AddIdentityServices(builder.Configuration, builder.Environment)
    .AddDalServices(builder.Configuration)
    .AddMigrationsServices(builder.Configuration);

builder.Services.AddHealthChecks();
builder.Services.AddCors();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

var app = builder.Build();

await app.InitialiseDatabases();

app.UseCors(x => x
    .WithOrigins(builder.Configuration["Security:AllowedOrigins"]?.Trim().Split(",") ?? Array.Empty<string>())
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseExceptionHandler(options => { });

app
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

app.MapHealthChecks("/healthz");

app.Run();