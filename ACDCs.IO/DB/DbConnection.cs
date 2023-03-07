using LiteDB;

namespace ACDCs.IO.DB;

public class DBConnection
{
    private readonly string _connectionString;

    public DBConnection(string dbname)
    {
        string dbdir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db", dbname);
        if (!Directory.Exists(dbdir))
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db"));
        }
        _connectionString = $"Filename={dbdir}";
    }

    public T? GetOrSet<T>(string collectionName, string keyName, string keyValue, T? newValue = default)
    {
        using LiteDatabase db = new(_connectionString);
        T? retValue = default;
        if (!db.CollectionExists(collectionName))
        {
            return retValue;
        }

        ILiteCollection<T>? col = db.GetCollection<T>(collectionName);
        retValue = col.FindOne(Query.EQ(keyName, keyValue));
        if (newValue == null)
        {
            return retValue;
        }

        if (retValue != null)
            col.DeleteMany(Query.EQ(keyName, keyValue));
        col.Insert(newValue);

        return retValue;
    }

    public List<T> Read<T>(string collectionName)
    {
        using LiteDatabase db = new(_connectionString);
        return db.CollectionExists(collectionName) ? db.GetCollection<T>(collectionName).FindAll().ToList() : new List<T>();
    }

    public void ReWrite<T>(List<T> items, string collectionName)
    {
        using LiteDatabase db = new(_connectionString);

        db.GetCollection<T>(collectionName).DeleteAll();
        db.GetCollection<T>(collectionName).Insert(items);
    }

    public void Write<T>(List<T> items, string collectionName)
    {
        using LiteDatabase db = new(_connectionString);

        db.GetCollection<T>(collectionName)
            .Insert(items);
    }
}
