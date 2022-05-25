using Microsoft.Extensions.DependencyInjection;
using Monq.Core.Localization.Models;

namespace Monq.Core.Localization.DependencyInjection.Extensions
{
    /// <summary>
    /// Методы расширения для работы с коллекцией сервисов.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Добавить локализацию атрибутов аннотации данных.
        /// </summary>
        /// <typeparam name="T">Реализация интерфейса вспомогательного класса для создания общего файла ресурсов локализации.</typeparam>
        /// <param name="builder"><see cref="IMvcBuilder"/></param>
        /// <returns></returns>
        public static IMvcBuilder AddMonqDataAnnotationsLocalization<T>(this IMvcBuilder builder)
            where T : ILocalizationResource
            => builder.AddDataAnnotationsLocalization(opt => opt.DataAnnotationLocalizerProvider = (type, factory) =>
                factory.Create(typeof(T)));
    }
}
