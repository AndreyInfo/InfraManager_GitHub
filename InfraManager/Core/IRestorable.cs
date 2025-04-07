namespace InfraManager.Core
{
    /// <summary>
    /// Интерфейс, поддерживаемый сущностями, реализующими сценарий восстанавливаемого внутреннего состояния
    /// </summary>
    public interface IRestorable
    {
        /// <summary>
        /// Инициирует сохранение текущего состояния сущности
        /// </summary>
        void AcceptChanges();

        /// <summary>
        /// Инициирует восстановление предыдущего состояния сущности
        /// </summary>
        void Restore();
    }
}
