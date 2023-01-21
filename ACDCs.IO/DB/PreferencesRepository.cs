namespace ACDCs.IO.DB
{
    public class PreferencesRepository
    {
        private readonly DBConnection _connection;

        public PreferencesRepository()
        {
            _connection = new DBConnection("preferences");
        }

        public T? GetPreference<T>(string key)
        {
            List<PreferenceSetting<T>> values = _connection.Read<PreferenceSetting<T>>("Pereferences");
            PreferenceSetting<T>? value = values.FirstOrDefault(p => p.Key == key);
            return value != default ? value.Value : default;
        }

        public PreferenceSetting<T> SetPreference<T>(string key, T value)
        {
            PreferenceSetting<T> setting = new(value, key);
            _connection.Write(new List<PreferenceSetting<T>> { setting }, "Preferences");
            return setting;
        }
    }
}
