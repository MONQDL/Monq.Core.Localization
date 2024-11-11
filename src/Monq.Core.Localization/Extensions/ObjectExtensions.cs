using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Monq.Core.Localization.Extensions;

/// <summary>
/// Методы расширения для работы с объектами.
/// </summary>
internal static class ObjectExtensions
{
    /// <summary>
    /// Создать глубокую копию объекта.
    /// </summary>
    /// <typeparam name="T">Тип копируемого объекта.</typeparam>
    /// <param name="source">Копируемый объект.</param>
    /// <returns>Дубликат исходного объекта.</returns>
    public static T? Duplicate<T>(this T? source)
        => (T?)Duplicate(source, typeof(T));

    /// <summary>
    /// Создать глубокую копию объекта.
    /// </summary>
    /// <param name="source">Копируемый объект.</param>
    /// <param name="sourceType">Тип копируемого объекта.</param>
    /// <returns>Дубликат исходного объекта.</returns>
    public static object? Duplicate(this object? source, Type sourceType)
    {
        if (source is null)
            return default;

        var json = JsonSerializer.Serialize(source, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles });
        var result = JsonSerializer.Deserialize(json, sourceType);

        return result;
    }
}
