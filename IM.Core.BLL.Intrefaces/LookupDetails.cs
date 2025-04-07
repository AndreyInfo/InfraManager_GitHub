namespace InfraManager.BLL
{
    public class LookupDetails<TKey> : LookupListItem<TKey>
        where TKey : struct
    {
        public byte[] RowVersion { get; init; }
    }
}
