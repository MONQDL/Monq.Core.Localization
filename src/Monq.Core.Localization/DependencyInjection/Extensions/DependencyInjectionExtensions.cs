using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Monq.Core.Localization.Configuration;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Методы расширения для работы с DI.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        static readonly List<CultureInfo> _supportedCultures = new()
        {
            new CultureInfo(Constants.CultureIds.En),
            new CultureInfo(Constants.CultureIds.Ru),
        };
        static readonly RequestLocalizationOptions _defaultOptions = new()
        {
            DefaultRequestCulture = new RequestCulture(Constants.CultureIds.En),
            SupportedCultures = _supportedCultures,
            SupportedUICultures = _supportedCultures
        };

        /// <summary>
        /// Использовать middleware для локализации.
        /// </summary>
        /// <param name="app">Конвейер конфигурации приложения.</param>
        /// <param name="options">Настройки локализации.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseMonqLocalization(this IApplicationBuilder app, RequestLocalizationOptions? options = null)
        {
            app.UseRequestLocalization(options ?? _defaultOptions);
            return app;
        }
    }
}
