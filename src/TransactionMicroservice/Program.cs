using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using TransactionMicroservice.Application;
using TransactionMicroservice.Infrastructure;
using TransactionMicroservice.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

// Testando o Banco de Dados
try {
    var client = app.Services.GetRequiredService<IMongoClient>();
    var database = client.GetDatabase("admin");
    
    var result = await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
    Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
} catch (Exception ex) {
    Console.WriteLine("Failed to connect to MongoDB: " + ex.Message);
    throw;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

