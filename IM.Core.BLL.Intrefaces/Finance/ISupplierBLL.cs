using Inframanager.BLL;
using System.Threading;
using System.Threading.Tasks;
using SupplierEntity = InfraManager.DAL.Finance.Supplier;


namespace InfraManager.BLL.Finance
{
    public interface ISupplierBLL
    {
        Task<SupplierDetails[]> GetDetailsArrayAsync(
            LookupListFilter filter,
            CancellationToken cancellationToken = default);

        Task<SupplierDetails[]> GetDetailsPageAsync(
            LookupListFilter filter,
            ClientPageFilter<SupplierEntity> pageFilter,
            CancellationToken cancellationToken = default);
    }
}
