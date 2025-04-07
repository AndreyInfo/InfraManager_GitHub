using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.Import;
using InfraManager.DAL.Import.CSV;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Import
{
    public class SchedulerBLL : IImportCSVBLL, ISelfRegisteredService<IImportCSVBLL>
    {
        private readonly IImportApi _api;
        private readonly IMapper _mapper;
        public SchedulerBLL(
            IImportApi api, 
            IMapper mapper)
        {
            _api = api;
            _mapper = mapper;
        }

        public async Task<CSVConfigurationTableAPI[]> GetConfigurationTableAsync(CancellationToken cancellationToken)
        {
            var configurationTable = await _api.GetConfigurationTableAsync(cancellationToken);
            return configurationTable;
        }

        public async Task<string> GetPathAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _api.GetPathAsync(id, cancellationToken);
        }

        public async Task UpdatePathAsync(Guid id, string path, CancellationToken cancellationToken)
        {
            await _api.UpdatePathAsync(id, path, cancellationToken);
        }


    }
}
