using System;

namespace Monq.Core.Localization.Infrastructure;

/// <summary>
/// Атрибут локализуемого свойства.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class LocalizablePropertyAttribute : Attribute
{
}
