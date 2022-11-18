using WebAppiTest.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextDefault(builder);
builder.Services.AddRouteOptionsDefault();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UserCorsDefault();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();