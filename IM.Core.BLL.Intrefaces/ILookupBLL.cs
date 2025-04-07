using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL
{
    public interface ILookupBLL<TListItem, TDetails, TData, TKey> 
        where TListItem : LookupListItem<TKey>
        where TDetails : LookupDetails<TKey>
        where TData : LookupData
        where TKey : struct
    {
        Task<TListItem[]> ListAsync(CancellationToken token = default);
        Task<TDetails> FindAsync(TKey id, CancellationToken token = default);
        Task<TDetails> AddAsync(TData model, CancellationToken token = default);
        Task<TDetails> UpdateAsync(TKey id, TData model, CancellationToken token = default);
        Task DeleteAsync(TKey id, CancellationToken token = default);
    }
}
