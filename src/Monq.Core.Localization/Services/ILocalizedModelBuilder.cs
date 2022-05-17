using System;
using System.Collections.Generic;

namespace Monq.Core.Localization.Services
{
    /// <summary>
    /// Интерфейс сервиса локализации моделей.
    /// </summary>
    public interface ILocalizedModelBuilder
    {
        /// <summary>
        /// Построить локализованную модель представления из исходной модели.
        /// </summary>
        /// <param name="source">Данные для локализации.</param>
        /// <param name="sourceType">Тип данных для локализации.</param>
        /// <returns></returns>
        object? Build(object source, Type sourceType);

        /// <summary>
        ///  Построить локализованную модель представления из исходной модели.
        /// </summary>
        /// <typeparam name="T">Тип данных для локализации.</typeparam>
        /// <param name="source">Данные для локализации.</param>
        /// <returns></returns>
        T? Build<T>(T? source);

        /// <summary>
        /// Построить список локализованных моделей представления из исходных моделей.
        /// </summary>
        /// <typeparam name="T">Тип данных для локализации.</typeparam>
        /// <param name="source">Данные для локализации.</param>
        /// <returns></returns>
        IEnumerable<T?> Build<T>(IEnumerable<T?> source);
    }
}
