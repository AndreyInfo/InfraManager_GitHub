using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Import;

public interface IGetValidTypesQuery
{
    Task<Guid[]> ExecuteAsync(Guid importSettingsID, CancellationToken token);
}