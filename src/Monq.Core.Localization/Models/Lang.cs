using System;
using System.Collections.Generic;

namespace Monq.Core.Localization.Models;

/// <summary>
/// Язык ресурсов локализации.
/// </summary>
public sealed class Lang : IEquatable<Lang>
{
    /// <summary>
    /// Идентификатор языка (RFC 4646).
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Справочник предметной области сервиса локали.
    /// </summary>
    public List<Resource> Resources { get; set; }
        = new List<Resource>();

    /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public bool Equals(Lang? other)
        => other is not null
        && Id == other.Id;
}
