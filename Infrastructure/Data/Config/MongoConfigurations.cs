namespace Data.Config;

public static class MongoConfigurations
{
    public static void RegisterAll()
    {
        OrderConfigure.Configure();
    }
}
