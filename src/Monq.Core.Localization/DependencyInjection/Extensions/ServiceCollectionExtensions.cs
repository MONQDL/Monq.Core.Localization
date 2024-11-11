using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Monq.Core.Localization.Configuration;
using Monq.Core.Localization.Models;
using Monq.Core.Localization.Services;
using Monq.Core.Localization.Services.Implementation;
using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Методы расширения для работы с коллекцией сервисов.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить локализацию через <see cref="DbContext"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов, зарегистрированных в DI.</param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextLocalization<T>(
        this IServiceCollection services)
        where T : LocalizationDbContext
    {
        services.AddScoped<IStringLocalizer, EfCoreStringLocalizer>();
        services.AddDbContext<LocalizationDbContext, T>();

        services = RegisterCommonServices(services);

        return services;
    }

    /// <summary>
    /// Добавить локализацию через файлы ресурсов.
    /// </summary>
    /// <typeparam name="T">Реализация интерфейса вспомогательного класса для создания общего файла ресурсов локализации.</typeparam>
    /// <param name="services">Коллекция сервисов, зарегистрированных в DI.</param>
    /// <param name="setupAction">Действие настройки локализации.</param>
    /// <returns></returns>
    public static IServiceCollection AddResourceLocalization<T>(
        this IServiceCollection services,
        Action<LocalizationOptions>? setupAction = null)
        where T : ILocalizationResource
    {
        setupAction ??= (opt) => opt.ResourcesPath = "Resources";
        services.AddLocalization(setupAction);

        services.AddSingleton<IStringLocalizer, ResourceStringLocalizer<T>>();

        services = RegisterCommonServices(services);

        return services;
    }

    static IServiceCollection RegisterCommonServices(IServiceCollection services)
    {
        services.TryAddTransient<ILocalizedModelBuilder, LocalizedModelBuilder>();

        return services;
    }
}
