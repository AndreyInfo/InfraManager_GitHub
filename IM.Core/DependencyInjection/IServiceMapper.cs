namespace InfraManager
{
    public interface IServiceMapper<TKey, TService> where TService : class
    {
        bool HasKey(TKey key);
        TService Map(TKey key);
    }
}
