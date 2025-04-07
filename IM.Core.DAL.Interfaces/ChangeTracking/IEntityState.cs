namespace InfraManager.DAL.ChangeTracking
{
    /// <summary>
    /// Этот интерфейс описывает состояние сущности
    /// </summary>
    public interface IEntityState
    {
        /// <summary>
        /// Возвращает значение свойства сущности
        /// </summary>
        /// <param name="property">Название свойства</param>
        /// <returns>Значение свойства</returns>
        object this[string property] { get; }

        /// <summary>
        /// Возвращает ссылку на связанный Immutable объект, который сам отслеживает свое состояние
        /// </summary>
        /// <param name="name">Наименование свойства</param>
        /// <returns>Ссылка на состояние ссылки</returns>
        object Reference(string name);
    }
}
