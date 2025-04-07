namespace InfraManager
{
    public interface ICatalog<TKey> where TKey : struct
    {
        public TKey ID { get; }
        public string Name { get; }
    }
}
