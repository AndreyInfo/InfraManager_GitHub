namespace Inframanager;

public enum ObjectAction
{
    /// <summary>
    /// Добавить
    /// </summary>
    Insert,
    /// <summary>
    /// Создание по аналогии
    /// </summary>
    InsertAs,
    /// <summary>
    /// Обновление
    /// </summary>
    Update,
    /// <summary>
    /// Удаление
    /// </summary>
    Delete,
    /// <summary>
    /// Получение массива
    /// </summary>
    ViewDetailsArray,
    /// <summary>
    /// Получение одной
    /// </summary>
    ViewDetails,
    /// <summary>
    /// Запланировать
    /// </summary>
    Plan,
    /// <summary>
    /// Выполнить
    /// </summary>
    Execute
}
