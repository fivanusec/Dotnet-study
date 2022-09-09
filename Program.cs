using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Dotnetrest.Data;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
)
.AddCookie();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsSpecs",
    builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(options => true)
            .AllowCredentials();
    });
});

builder.Services.AddDbContext<ApplicationDatabaseContext>(options =>
{
    options.UseSqlite(configuration.GetConnectionString("sqlite"));
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsSpecs");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
