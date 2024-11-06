using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Monq.Core.Localization.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Monq.Core.Localization.Services.Implementation;

/// <summary>
/// Сервис локализации через БД.
/// </summary>
internal class EfCoreStringLocalizer : IStringLocalizer
{
    readonly LocalizationDbContext _context;
    readonly IMemoryCache _cache;

    static readonly TimeSpan _cacheExpiresAfter = TimeSpan.FromMinutes(5);

    string CultureResourcesKey => CultureInfo.CurrentCulture.Name + "-resources";

    /// <summary>
    /// Конструктор сервиса локализации запросов на основе <see cref="IStringLocalizer"/>.
    /// Создаёт новый экземпляр <see cref="EfCoreStringLocalizer"/>.
    /// </summary>
    public EfCoreStringLocalizer(
        LocalizationDbContext context,
        IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    /// <inheritdoc/>
    public LocalizedString this[string name] => Get(name);

    /// <inheritdoc/>
    public LocalizedString this[string name, params object[] arguments] => Get(name);

    /// <inheritdoc/>
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var cultureName = CultureInfo.CurrentCulture.Name;
        var result = _cache.GetOrCreate(CultureResourcesKey, cache =>
        {
            cache.AbsoluteExpirationRelativeToNow = _cacheExpiresAfter;
            return GetAllResources(cultureName);
        });
        return result.Select(x => x.Value);
    }

    /// <inheritdoc/>
    public IStringLocalizer WithCulture(CultureInfo culture)
    {
        CultureInfo.DefaultThreadCurrentCulture = culture;
        return new EfCoreStringLocalizer(_context, _cache);
    }

    string? GetString(string name)
    {
        var cultureName = CultureInfo.CurrentCulture.Name;
        var result = _cache.GetOrCreate(CultureResourcesKey, cache =>
        {
            cache.AbsoluteExpirationRelativeToNow = _cacheExpiresAfter;
            return GetAllResources(cultureName);
        });
        return result.TryGetValue(name, out var value) ? value.Value : null;
    }

    Dictionary<string, LocalizedString> GetAllResources(string cultureName)
    {
        var resources = _context.Resources
                            .AsNoTracking()
                            .Where(x => x.LangId == cultureName)
                            .ToList();

        return resources
            .Select(x => new LocalizedString(x.Key, x.Value ?? string.Empty))
            .ToDictionary(x => x.Name);
    }

    LocalizedString Get(string name)
    {
        var result = GetString(name);
        return result is null ? Fallback(name) : new LocalizedString(name, result);
    }

    static LocalizedString Fallback(string name) => new(name, name, true);
}
