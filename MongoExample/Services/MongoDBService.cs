using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoExample.Services;

public class MongoDBService {
    private readonly IMongoCollection<Feedback> _feedbackCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings){
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _feedbackCollection = database.GetCollection<Feedback>(mongoDBSettings.Value.CollectionName);
    }

    public async Task CreateAsync(Feedback feedback) {
        await _feedbackCollection.InsertOneAsync(feedback);
        return;
    }

    public async Task<List<Feedback>> GetAsync(){
        return await _feedbackCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task AddToFeedbackAsync(string id, string feedback){
        FilterDefinition<Feedback> filter = Builders<Feedback>.Filter.Eq("Id", id);
        UpdateDefinition<Feedback> update = Builders<Feedback>.Update.AddToSet<string>("feedback", feedback);
        await _feedbackCollection.UpdateOneAsync(filter, update);
        return;
    }

    public async Task DeleteAsync(string id){
        FilterDefinition<Feedback> filter = Builders<Feedback>.Filter.Eq("Id", id);
        await _feedbackCollection.DeleteOneAsync(filter);
        return;
    }

}