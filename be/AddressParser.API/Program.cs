using AddressParser.Application.Interfaces;
using AddressParser.Application.Services;
using AddressParser.Infrastructure.Parsers;
using AddressParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

const string AllowLocalhost = "AllowLocalhost3000";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AddressDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy(AllowLocalhost, policy =>
    {
        policy.WithOrigins("http://localhost:3000")  
              .AllowAnyHeader()                      
              .AllowAnyMethod();                     
    });
});
builder.Services.AddScoped<IAddressParser, SimpleAddressParser>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IParseStatisticsRepository, ParseStatisticsRepository>();
builder.Services.AddScoped<IParseStatsService, ParseStatsService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddressParser API V1");
        c.RoutePrefix = string.Empty;
    });
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(AllowLocalhost);
app.UseAuthorization();

app.MapControllers();

app.Run();
