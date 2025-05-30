﻿using System.Text.Json;

namespace Stellaway.Common.Helpers;

public class JsonSerializerUtils
{
    public static JsonSerializerOptions GetGlobalJsonSerializerOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //Converters = { new JsonStringEnumConverter(), },
            //ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }

    public static string Serialize<TValue>(TValue value, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(value, options ?? GetGlobalJsonSerializerOptions());
    }

    public static TValue? Deserialize<TValue>(string json, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Deserialize<TValue>(json, options ?? GetGlobalJsonSerializerOptions());
    }

}