using TransactionMicroservice.Domain.Entities;
using TransactionMicroservice.Domain.Interfaces;
using TransactionMicroservice.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TransactionMicroservice.Infrastructure.Repositories;

public class DbTransacationRepository : ITransactionRepository
{
    private readonly IMongoCollection<Transaction> _collection;

    public DbTransacationRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
    {
        var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<Transaction>(settings.Value.CollectionName);
    }
    
    public async Task CreateAsync(Transaction transaction)
    {
        await _collection.InsertOneAsync(transaction);
    }
    
    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(String transactionId)
    {
        var filter = Builders<Transaction>.Filter.Eq(t => t.Id, transactionId);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}