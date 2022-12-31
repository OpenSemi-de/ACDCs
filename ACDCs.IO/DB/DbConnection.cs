using LiteDB;

namespace ACDCs.IO.DB
{
    public class DBConnection
    {
        private readonly string _connectionString;

        public DBConnection(string dbname)
        {
            string dbdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db", dbname);
            if (!Directory.Exists(dbdir))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db"));
            }
            _connectionString = $"Filename={dbdir}";
        }

        public List<T> Read<T>()
        {
            using LiteDatabase db = new(_connectionString);
            if (db.GetCollection<T>() != null)
                return db.GetCollection<T>().FindAll().ToList();

            return new();
        }

        public void Write<T>(List<T> items)
        {
            using LiteDatabase db = new(_connectionString);

            db.GetCollection<T>()
                .Insert(items);
        }
    }
}
