using System.Diagnostics.CodeAnalysis;
using ACDCs.Data.ACDCs.Components.BJT;
using ACDCs.Data.ACDCs.Components.Diode;
using ACDCs.Data.ACDCs.Interfaces;
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

    public List<T> Read<T>(string collectionName)
    {
        using LiteDatabase db = new(_connectionString);
        return db.CollectionExists(collectionName) ? db.GetCollection<T>(collectionName).FindAll().ToList() : new List<T>();
    }

    public void Write<T>(List<T> items, string collectionName)
    {
        using LiteDatabase db = new(_connectionString);

        db.GetCollection<T>(collectionName)
            .Insert(items);
        db.Dispose();
    }
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public class DefaultModelRepository
{
    private readonly DBConnection _connection;

    public DefaultModelRepository()
    {
        _connection = new DBConnection("default");
    }

    public List<T> GetModels<T>(string type)
    {
        List<IElectronicComponent> models = new();

        switch (type.ToLower())
        {
            case "pnp":
            case "npn":
                models = GetModels()
                    .Where(c => c is Bjt bjt && bjt.TypeName == type.ToLower())
                    .ToList();
                break;

            case "diode":
                models = GetModels()
                    .Where(c => c is Diode)
                    .ToList();
                break;
        }

        return models.Cast<T>().ToList();
    }

    public List<IElectronicComponent> GetModels()
    {
        List<IElectronicComponent> models = _connection.Read<IElectronicComponent>("Components");
        return models;
    }

    public void Write(List<IElectronicComponent?> newComponents)
    {
        newComponents = newComponents.Where(c => c != null).ToList();
        _connection.Write(newComponents, "Components");
    }
}
