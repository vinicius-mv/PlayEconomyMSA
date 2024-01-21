namespace Play.Catalog.Service.Settings;

public class MongoDbSettings
{
    public string Host { get; init; }

    public string Port { get; init; }

    public string ConnectionString => $"mongodb://{Host}:{Port}";
}
