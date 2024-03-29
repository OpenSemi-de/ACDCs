﻿namespace ACDCs.IO.DB
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
            return setting?.Value;
        }

        public PreferenceSetting SetPreference(string key, object value)
        {
            PreferenceSetting setting = new() { Key = key, Value = value, TypeName = value.GetType().Name };

            object? existingValue = GetPreference(key);
            if (existingValue != null)
            {
                _connection.GetOrSet("Preferences", "Key", key, setting);
            }
            else
            {
                _connection.Write(new List<PreferenceSetting> { setting }, "Preferences");
            }

            return setting;
        }
    }
}
