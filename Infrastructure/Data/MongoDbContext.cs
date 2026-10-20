using Core.Entitys;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Data;

public class MongoDbContext
{
    public IMongoCollection<Order> Orders{get;set;}

    IConfiguration _configuration;

    public MongoDbContext(IConfiguration configuration)
    {
        _configuration=configuration;

        var client=new MongoClient(_configuration.GetValue<string>("MongoDb:ConnectionString"));
        var db=client.GetDatabase(_configuration.GetValue<string>("MongoDb:DbName"));

        Orders=db.GetCollection<Order>(_configuration.GetValue<string>("MongoDb:CollectionName:Order"));
    }
}
