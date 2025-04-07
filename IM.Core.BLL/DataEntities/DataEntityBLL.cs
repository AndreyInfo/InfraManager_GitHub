using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.DataEntities.DTO;
using InfraManager.DAL.ConfigurationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.DataEntities
{
    internal class DataEntityBLL :
        IDataEntityBLL,
        ISelfRegisteredService<IDataEntityBLL>
    {
        private readonly IGetEntityBLL<Guid, DataEntity, DataEntityDetails> _dataEntities;

        public DataEntityBLL(IGetEntityBLL<Guid, DataEntity, DataEntityDetails> dataEntities)
        {
            _dataEntities = dataEntities;
        }

        public async Task<DataEntityDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dataEntities.DetailsAsync(id, cancellationToken);
        }
    }

}
