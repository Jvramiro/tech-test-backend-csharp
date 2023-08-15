using TestJrAPI.Data;
using TestJrAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<SqlContext>(builder.Configuration["ConnectionStrings:SQLServer"]);

builder.Services.AddScoped<DatabaseService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
