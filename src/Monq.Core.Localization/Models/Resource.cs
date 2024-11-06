using System;

namespace Monq.Core.Localization.Models;

/// <summary>
/// Ресурс локализации строки исходного кода (БД).
/// </summary>
public sealed class Resource : IEquatable<Resource>
{
    /// <summary>
    /// Идентификатор языка (RFC 4646) ресурса.
    /// </summary>
    public string? LangId { get; set; }

    /// <summary>
    /// Ключ ресурса (переводимая строка).
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Значение ключа ресурса в локали языка.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Язык ресурса.
    /// </summary>
    public Lang? Lang { get; set; }

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(Resource? other)
        => other is not null
        && LangId == other.LangId
        && Key == other.Key
        && Value == other.Value;
}
