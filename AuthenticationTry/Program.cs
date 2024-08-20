using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationTry.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AuthenticationTryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthenticationTryContext") ?? throw new InvalidOperationException("Connection string 'AuthenticationTryContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<AuthenticationTry.Middlewares.Middleware1>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
