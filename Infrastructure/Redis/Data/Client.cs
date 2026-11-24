using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Redis.Data;

public class Client
{
    private readonly IDatabase _db;

    public Client(IConfiguration configuration)
    {
        string conn=configuration.GetConnectionString("Redis")!;

        ConnectionMultiplexer multiplexer=ConnectionMultiplexer.Connect(conn);
        _db=multiplexer.GetDatabase();
    }
    public IDatabase GetDatabase()=>_db;
}
