namespace ACDCs.IO.DB
{
    public class PreferencesRepository
    {
        private readonly DBConnection _connection;

        public PreferencesRepository()
        {
            _connection = new DBConnection("preferences");
        }

        public void Delete(string key)
        {
        }

        public object? GetPreference(string key)
        {
            PreferenceSetting? setting = _connection.GetOrSet<PreferenceSetting>("Preferences", "Key", key);
            if (setting == null)
            {
                return null;
            }

            return setting.Value;
        }

        public PreferenceSetting SetPreference(string key, object value)
        {
            PreferenceSetting setting = new() { Key = key, Value = value, TypeName = value.GetType().Name };

            var existingValue = GetPreference(key);
            if (existingValue != null)
            {
                _connection.GetOrSet("Preferences", "Key", key, value);
            }
            else
            {
                _connection.Write(new List<PreferenceSetting> { setting }, "Preferences");
            }

            return setting;
        }
    }
}
