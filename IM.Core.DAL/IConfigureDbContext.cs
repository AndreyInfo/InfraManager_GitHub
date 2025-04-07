using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL
{
    public interface IConfigureDbContext // TODO: Переходим на базовые IEntityTypeConfiguration 
    {
        /// <summary>
        /// Конфигурирует параметры соединения с СуБД
        /// </summary>
        /// <param name="optionsBuilder">Построитель опций контекста данных (ef core)</param>
        void Configure(DbContextOptionsBuilder optionsBuilder);

        string Schema { get; }

    }
}
