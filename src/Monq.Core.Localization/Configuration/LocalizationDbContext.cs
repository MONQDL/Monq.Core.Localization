using Microsoft.EntityFrameworkCore;
using Monq.Core.Localization.Models;

namespace Monq.Core.Localization.Configuration
{
    /// <summary>
    /// Контекст БД для локализации.
    /// </summary>
    public class LocalizationDbContext : DbContext
    {
        /// <summary>
        /// Языки ресурсов локализации.
        /// </summary>
        public virtual DbSet<Lang> Langs => Set<Lang>();

        /// <summary>
        /// Ресурсы локализации.
        /// </summary>
        public virtual DbSet<Resource> Resources => Set<Resource>();

        /// <summary>
        /// Конструктор контекста БД для локализации.
        /// </summary>
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Конструктор контекста БД для локализации.
        /// </summary>
        protected LocalizationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
