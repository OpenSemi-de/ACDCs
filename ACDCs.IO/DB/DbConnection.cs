using LiteDB;

namespace ACDCs.IO.DB
{
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
            if (db.CollectionExists(collectionName))
                return db.GetCollection<T>(collectionName).FindAll().ToList();

            return new();
        }

        public void Write<T>(List<T> items, string collectionName)
        {
            using LiteDatabase db = new(_connectionString);

            db.GetCollection<T>(collectionName)
                .Insert(items);
            db.Dispose();
        }
    }
}
