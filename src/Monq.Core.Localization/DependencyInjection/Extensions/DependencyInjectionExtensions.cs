using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Monq.Core.Localization.Configuration;
using System.Globalization;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Методы расширения для работы с DI.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Использовать middleware для локализации.
        /// </summary>
        /// <param name="app">Конвейер конфигурации приложения.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseMonqLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo(Constants.CultureIds.Ru),
                new CultureInfo(Constants.CultureIds.En),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(Constants.CultureIds.En),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }
    }
}
