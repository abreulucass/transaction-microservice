using MongoDB.Driver;
using TransactionMicroservice.Application;
using TransactionMicroservice.Infrastructure;
using TransactionMicroservice.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// -------------- Configuracao do Banco de Dados ------------//
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var config = builder.Configuration.GetConnectionString("MongoDb");
    return new MongoClient(config);
});
// --------------------------------------------------------- //

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();

