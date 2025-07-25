using Azure.Messaging.ServiceBus;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TransactionMicroservice.Infrastructure;

public class StartupDiagnostics
{
    public static async Task TestMongoConnectionAsync(IServiceProvider services)
    {
        try
        {
            var client = services.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase("admin");

            var result = await database.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
            Console.WriteLine("Conectado ao MongoDB com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Falha ao conectar ao MongoDB: " + ex.Message);
            throw;
        }
    }

    public static async Task TestAzureServiceBusConnectionAsync(IServiceProvider services)
    {
        try
        {
            var config = services.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("AzureBusConnection");
            var queueName = config.GetSection("AzureServiceBus")["QueueName"];

            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(queueName); 
            _ = sender.EntityPath; 

            Console.WriteLine("Conectado ao Azure Service Bus com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Falha ao conectar ao Azure Service Bus: " + ex.Message);
            throw;
        }
    }
}