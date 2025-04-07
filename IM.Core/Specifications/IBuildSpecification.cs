namespace Inframanager
{
    /// <summary>
    /// Этот интерфейс описывает построителя спецификации (или спецификацию, зависящую от фильтра)
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности</typeparam>
    /// <typeparam name="TFilter">Тип фильтра</typeparam>
    public interface IBuildSpecification<TEntity, TFilter>
    {
        /// <summary>
        /// Строит спецификацию TEntity, зависящую от фильтра TFilter
        /// </summary>
        /// <param name="filterBy">Фильтр</param>
        /// <returns>Ссылка на спецификацию</returns>
        Specification<TEntity> Build(TFilter filterBy);
    }
}
