namespace InfraManager.BLL
{
    public class LookupListItem<TKey> where TKey : struct
    {
        public TKey ID { get; init; }
        public string Name { get; init; }
    }
}
