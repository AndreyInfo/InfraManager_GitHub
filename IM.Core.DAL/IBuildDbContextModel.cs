using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот интерфейс описывает построителя ORM-конфигурации для конкретной СуБД
    /// </summary>
    public interface IBuildDbContextModel
    {
        void BuildModel(ModelBuilder modelBuilder);
    }
}
