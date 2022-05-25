using Monq.Core.Localization.Models;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// Методы расширения для работы со строителем моделей контекста БД.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Сконфигурировать контекст БД для локализации.
        /// </summary>
        /// <param name="modelBuilder">Строитель моделей.</param>
        /// <returns></returns>
        public static ModelBuilder ConfigureLocalizationContext(
            this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lang>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .HasMaxLength(sbyte.MaxValue);

                entity
                    .HasMany(x => x.Resources)
                    .WithOne(x => x.Lang)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(x => new { x.LangId, x.Key });

                entity.Property(x => x.Key)
                    .HasMaxLength(sbyte.MaxValue)
                    .IsRequired();

                entity.Property(x => x.Value)
                    .HasMaxLength(byte.MaxValue);

                // Индекс по ключу сущности получаем автоматически, но собственный ключ тоже удобно
                // иметь в качестве дополнительного индекса.
                entity.HasIndex(x => x.Key);
            });

            return modelBuilder;
        }
    }
}
