namespace InfraManager.DAL
{
    /// <summary>
    /// Этот интерфейс описывает стратегию удаление объектов из источника данных
    /// </summary>
    /// <typeparam name="TEntity">Тип Entity</typeparam>
    public interface IDeleteStrategy<TEntity>
    {
        /// <summary>
        /// Удаляет сущность в соответствии с конкретной стратегией
        /// (физическое удаление, логическое удаление, каскадное удаление)
        /// </summary>
        /// <param name="entity">Объект, который необходимо удалить</param>
        void Delete(TEntity entity);
    }
}
