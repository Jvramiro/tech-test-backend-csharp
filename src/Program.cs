using TestJrAPI.Data;
using TestJrAPI.Interfaces;
using TestJrAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<SqlContext>(builder.Configuration["ConnectionStrings:SQLServer"]);
builder.Services.Configure<MongoDBContext>(builder.Configuration);
builder.Services.AddScoped<MongoDBContext>();

builder.Services.Configure<FileContext>(builder.Configuration);
builder.Services.AddScoped<FileContext>();

builder.Services.AddScoped<IDatabaseService, DatabaseService>();

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
