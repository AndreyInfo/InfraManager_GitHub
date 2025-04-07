namespace InfraManager
{
    /// <summary>
    /// Этот интерфейс не имеет членов и используется для автоматической регистрации типа в коллекции сервисов
    /// </summary>
    /// <typeparam name="TService">Тип реализуемого сервиса</typeparam>
    public interface ISelfRegisteredService<TService>
    {
    }
}
