using System.Text.Json;
using System.Text.Json.Serialization;

namespace TransactionMicroservice.Helpers;

public class JsonHelper
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public static string ToJson(object obj)
        => JsonSerializer.Serialize(obj, _options);
}