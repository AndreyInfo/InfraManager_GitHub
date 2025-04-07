namespace InfraManager
{
    // TODO изменить логику, чтобы сделать ID { get; init; }
    public class Catalog<TKey> : ICatalog<TKey> where TKey : struct
    {
        public TKey ID { get; set; } // TODO: нарушение принципа инкапсуляции
        public string Name { get; set; }
    }
}
