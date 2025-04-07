using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL;

internal class ServiceContractLookupQuery : ILookupQuery
{
    private readonly DbSet<ServiceContract> _serviceContracts;

    public ServiceContractLookupQuery(DbSet<ServiceContract> serviceContracts)
    {
        _serviceContracts = serviceContracts;
    }

    public async Task<ValueData[]> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Array.ConvertAll(
            await _serviceContracts.AsNoTracking()
                .Select(serviceContract => new
                {
                    ID = serviceContract.ID,
                    ServiceContractNumber = serviceContract.Number,
                }).ToArrayAsync(cancellationToken),
            item => new ValueData
            {
                ID = item.ID.ToString(),
                Info = item.ServiceContractNumber.ToString(),
            });
    }
}