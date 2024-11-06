using Microsoft.Extensions.Localization;
using Monq.Core.Localization.Extensions;
using Monq.Core.Localization.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Monq.Core.Localization.Services.Implementation;

/// <summary>
/// Реализация интерфейса сервиса локализации моделей.
/// </summary>
public class LocalizedModelBuilder : ILocalizedModelBuilder
{
    /// <summary>
    /// Доступные для локализации типы.
    /// </summary>
    static readonly Type[] _localizableTypes =
    {
        typeof(string),
        typeof(IEnumerable<string>)
    };

    readonly IStringLocalizer _localizer;

    /// <summary>
    /// Конструктор реализации интерфейса сервиса локализации моделей.
    /// </summary>
    public LocalizedModelBuilder(
        IStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    /// <inheritdoc/>
    public object? Build(object? source, Type sourceType)
    {
        if (source is null)
            return default;

        var sourceCopy = source.Duplicate(sourceType);

        var properties = sourceType.GetProperties()
            .Where(p => p.GetCustomAttributes(typeof(LocalizablePropertyAttribute), false).Any())
            .ToList();
        foreach (var property in properties)
        {
            if (_localizableTypes.Contains(property.PropertyType))
            {
                sourceCopy = Localize(sourceCopy, property);
                continue;
            }

            if (property.PropertyType.IsGenericType
                && property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var collection = (IEnumerable?)property.GetValue(sourceCopy);
                if (collection is null)
                    continue;

                var type = collection.GetType().GetElementType();
                if (type is null)
                    continue;

                var instance = (IList?)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
                if (instance is null)
                    continue;

                foreach (var item in collection)
                    instance.Add(Build(item, type));
                property.SetValue(sourceCopy, instance);
                continue;
            }

            if (property.PropertyType.IsClass)
            {
                var value = property.GetValue(sourceCopy);
                if (value is null)
                    continue;
                var localizedValue = Build(value, value.GetType());
                property.SetValue(sourceCopy, localizedValue);
            }
        }

        return sourceCopy;
    }

    /// <inheritdoc/>
    public T? Build<T>(T? source)
        => (T?)Build(source, typeof(T));

    /// <inheritdoc/>
    public IEnumerable<T?> Build<T>(IEnumerable<T?> source)
    {
        if (!source.Any())
            return Array.Empty<T>();

        var localizedItems = new List<T?>();
        foreach (var item in source)
            localizedItems.Add(Build(item));

        return localizedItems;
    }

    T Localize<T>(T localizedSource, PropertyInfo property)
    {
        if (property.PropertyType == typeof(string))
        {
            var resource = property.GetValue(localizedSource)?.ToString();
            if (!string.IsNullOrEmpty(resource))
                property.SetValue(localizedSource,
                    (_localizer[resource]?.Value) is null ? resource : _localizer[resource].Value);

            return localizedSource;
        }

        if (property.PropertyType == typeof(IEnumerable<string>)
            && property.GetValue(localizedSource) is not null)
        {
            var resources = (IEnumerable<string>?)property.GetValue(localizedSource);
            if (resources?.Any() == true)
            {
                var newResources = resources.Select(resource =>
                    _localizer[resource].Value).ToList();
                property.SetValue(localizedSource, newResources.AsEnumerable());
            }

            return localizedSource;
        }

        return localizedSource;
    }
}
