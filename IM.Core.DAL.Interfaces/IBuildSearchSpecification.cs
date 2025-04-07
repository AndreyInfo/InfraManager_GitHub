using Inframanager;

namespace InfraManager.DAL
{
    /// <summary>
    /// Этот интерфейс описывает построителя поисковой спецификации
    /// Строит спецификацию объектов типа TEntity, удовлетворяющих поисковому запросу
    /// Например: x => EF.Functions.Like(x.Name, searchText)
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    public interface IBuildSearchSpecification<TEntity> : IBuildSpecification<TEntity, string>
    {
    }
}
