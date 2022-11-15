using Microsoft.EntityFrameworkCore;
using WebAppiTest.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyDataBaseContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});
builder.Services.Configure<RouteOptions>(config =>
{
    config.LowercaseQueryStrings = true;
    config.LowercaseUrls = true;
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(config => config.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().WithMethods("PUT", "DELETE", "GET", "POST"));
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();