namespace InfraManager.DAL.DeleteStrategies
{
    /// <summary>
    /// Этот сервис реализует поведение подобное поведению Foreign Key при удалении родительской сущности
    /// (не использовать если в БД можно сделать FK и настроить эту логику там)
    /// </summary>
    /// <typeparam name="T">Тип родительской сущности</typeparam>
    internal interface IDependentDeleteStrategy<T>
    {
        /// <summary>
        /// Вызывается перед физическим удалением сущности
        /// </summary>
        /// <param name="entity">Ссылка родительской сущности, которую удаляют</param>
        void OnDelete(T entity);
    }
}
