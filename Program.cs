using SqliteProvider.Implementations;
using SqliteProvider.Interfaces;
using SqliteProvider.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbInit, DbInit>();
builder.Services.AddSingleton<ITableInit, TableInit>();
builder.Services.AddScoped<CsvService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>(sr => new DatabaseService("Data Source = database.db"));
builder.Services.AddScoped<IOrganizatonRepository, OrganizationRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
using (var serviceScope = app.Services.CreateScope())
{
	var services = serviceScope.ServiceProvider;

	var myDependency = services.GetRequiredService<IDbInit>();
	myDependency.EnsureDbAndTablesCreated();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
