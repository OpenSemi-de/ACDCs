namespace ACDCs.Interfaces;

using Newtonsoft.Json;

/// <summary>
/// Extension to convert any object to json.
/// </summary>
public static class JsonExtension
{
    private static readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented,
    };

    /// <summary>
    /// Converts to json.
    /// </summary>
    /// <param name="self">The self.</param>
    /// <returns></returns>
    public static string ToJson(this object self)
    {
        return JsonConvert.SerializeObject(self, _settings);
    }

    /// <summary>
    /// Converts to T from Json.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self">The self.</param>
    /// <returns></returns>
    public static T? ToObjectFromJson<T>(this string self)
    {
        return JsonConvert.DeserializeObject<T>(self, _settings) ?? default;
    }
}