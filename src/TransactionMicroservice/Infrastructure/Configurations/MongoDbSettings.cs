namespace TransactionMicroservice.Infrastructure.Configurations;

public class MongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string CollectionName { get; set; }
}