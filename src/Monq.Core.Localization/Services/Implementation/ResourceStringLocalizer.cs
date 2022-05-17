﻿using Microsoft.Extensions.Localization;
using Monq.Core.Localization.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Monq.Core.Localization.Services.Implementation
{
    /// <summary>
    /// Обёртка сервиса локализации через файлы ресурсов.
    /// </summary>
    /// <typeparam name="T">Реализация интерфейса вспомогательного класса для создания общего файла ресурсов локализации.</typeparam>
    internal class ResourceStringLocalizer<T> : IStringLocalizer
        where T : ILocalizationResource
    {
        readonly IStringLocalizer<T> _localizer;

        /// <summary>
        /// Конструктор обёртки сервиса локализации через файлы ресурсов.
        /// </summary>
        public ResourceStringLocalizer(IStringLocalizer<T> localizer)
            => _localizer = localizer;

        /// <inheritdoc/>
        public LocalizedString this[string name] => _localizer.GetString(name);

        /// <inheritdoc/>
        public LocalizedString this[string name, params object[] arguments] => _localizer.GetString(name, arguments);

        /// <inheritdoc/>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
            => _localizer.GetAllStrings(includeParentCultures);

#if NETCOREAPP3_1
        /// <inheritdoc/>
        [Obsolete("This method is obsolete. Use `CurrentCulture` and `CurrentUICulture` instead.")]
        public IStringLocalizer WithCulture(CultureInfo culture) => _localizer.WithCulture(culture);
#endif
    }
}
