using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Snmp;
using System;
using System.Linq;

namespace InfraManager.BLL.Snmp
{
    public class SnmpDeviceModelQueryBuilder :
        IBuildEntityQuery<SnmpDeviceModel, SnmpDeviceModelDetails, SnmpDeviceModelFilter>,
        ISelfRegisteredService<IBuildEntityQuery<SnmpDeviceModel, SnmpDeviceModelDetails, SnmpDeviceModelFilter>>
    {
        private readonly IReadonlyRepository<SnmpDeviceModel> _repository;

        public SnmpDeviceModelQueryBuilder(IReadonlyRepository<SnmpDeviceModel> repository)
        {
            _repository = repository;
        }

        public IExecutableQuery<SnmpDeviceModel> Query(SnmpDeviceModelFilter filter)
        {
            var query = _repository.Query();

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                query = query.Where(m => m.ModelName.ToLower() == filter.SearchString.ToLower());
            }

            if(filter.ModelIDs != null && filter.ModelIDs.Any())
            {
                query = query.Where(m => filter.ModelIDs.Contains(m.ModelID));
            }

            if (!string.IsNullOrWhiteSpace(filter.SysObjectIDValue))
            {
                query = query.Where(m => m.SysObjectIDValue.ToLower() == filter.SysObjectIDValue.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(filter.DescriptionTag))
            {
                query = query.Where(m => m.DescriptionTag.ToLower() == filter.DescriptionTag.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(filter.OIDValue))
            {
                query = query.Where(m => m.OIDValue.ToLower() == filter.OIDValue.ToLower());
                query = query.Where(m => m.OID.ToLower() == filter.OIDValue.ToLower());
            }

            return query;
        }
    }
}
