using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.ConfigurationUnit.DTO;
using InfraManager.BLL.DataEntities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ConfigurationUnit
{
    internal class ConfigurationUnitBLL :
        IConfigurationUnitBLL,
        ISelfRegisteredService<IConfigurationUnitBLL>
    {
        private readonly IGetEntityBLL<Guid, DAL.Configuration.ConfigurationUnit, ConfigurationUnitDetails> _configurationUnits;

        public ConfigurationUnitBLL(IGetEntityBLL<Guid, DAL.Configuration.ConfigurationUnit, ConfigurationUnitDetails> ConfigurationUnits)
        {
            _configurationUnits = ConfigurationUnits;
        }

        public async Task<ConfigurationUnitDetails> DetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _configurationUnits.DetailsAsync(id, cancellationToken);
        }
    }

}
