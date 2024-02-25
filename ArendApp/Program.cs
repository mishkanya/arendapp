using ArendApp;
using ArendApp.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



var connection = builder.Configuration.GetConnectionString("DefaultConnection");

if(string.IsNullOrWhiteSpace(connection))
{

    string workingDirectory = Environment.CurrentDirectory;

    var folder = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(folder);

    path = workingDirectory;

    var dbPath = System.IO.Path.Join(path, "blogging.db");
    connection = $"Data Source={dbPath}";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connection));


builder.Services.AddScoped<CodeSender>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
