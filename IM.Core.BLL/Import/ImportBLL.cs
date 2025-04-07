using AutoMapper;
using IM.Core.Import.BLL.Interface.Import;
using InfraManager.DAL.Import;
using InfraManager.ServiceBase.WebApiModes;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.Import
{
    public class ImportBLL : IImportBLL, ISelfRegisteredService<IImportBLL>
    {
        private readonly IImportApi _api;
       
        public ImportBLL(IImportApi api)
        {
            _api = api;
            
        }

        public async Task<ImportTasksDetails[]> GetImportTasksAsync(CancellationToken cancellationToken)
        {
            var importDetails = await _api.GetImportTasksAsync(cancellationToken);
            return importDetails;
        }

        public async Task<ImportMainTabDetails> GetMainDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.GetMainDetailsAsync(id, cancellationToken);
        }


        public async Task<Guid> CreateMainDetailsAsync(ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken)
        {
            return await _api.CreateMainDetailsAsync(mainTabDetails, cancellationToken);
        }

        public async Task UpdateMainDetailsAsync(Guid id, ImportMainTabDetails mainTabDetails, CancellationToken cancellationToken)
        {
            await _api.UpdateMainDetailsAsync(id, mainTabDetails, cancellationToken);
        }

        public async Task<DeleteDetails> DeleteTaskAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.DeleteTaskAsync(id, cancellationToken);
        }

        public async Task<AdditionalTabDetails> GetAdditionalDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.GetAdditionalDetailsAsync(id, cancellationToken);
        }

        public async Task UpdateAdditionalDetailsAsync(Guid id, AdditionalTabData settings, CancellationToken cancellationToken)
        {
            await _api.UpdateAdditionalDetailsAsync(id, settings, cancellationToken);
        }

    }
}
