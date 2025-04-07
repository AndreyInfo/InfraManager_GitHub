using AutoMapper;
using IM.Core.DM.BLL.Interfaces;
using IM.Core.Import.BLL.Interface.Import;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using InfraManager;
using InfraManager.DAL;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.ServiceBase.ImportService.LdapModels;
using Newtonsoft.Json;

namespace IM.Core.Import.BLL.Ldap.Import;

public class UIADConfigurationInsertQuery:IInsertQuery<UIADConfigurationsDetails,UIADConfiguration>
{
    private readonly IUIADIMFieldConcordancesBLL _fieldBLL;
    private readonly IRepository<UIADConfiguration> _configurations;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UIADConfigurationInsertQuery(IUIADIMFieldConcordancesBLL fieldBLL,
        IRepository<UIADConfiguration> configurations,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _fieldBLL = fieldBLL;
        _configurations = configurations;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<UIADConfiguration> AddAsync(UIADConfigurationsDetails model, CancellationToken token)
    {
        
        var entity = _mapper.Map<UIADConfiguration>(model);
        
        _configurations.Insert(entity);
        
        await _unitOfWork.SaveAsync(token);
        
        var configurationFields = JsonConvert.DeserializeObject<FieldConfiguration[]>(model.Configuration);

        
        if (configurationFields != null)
        {
            foreach (var configurationField in configurationFields)
            {
                if (!configurationField.FieldEnum.HasValue)
                    continue;
                
                var configData = new UIADIMFieldConcordanceInsertDetails(adConfigurationID: entity.ID,
                    adClassID: configurationField.ClassID, imFieldID: (long) configurationField.FieldEnum,
                    expression: configurationField.Value);
                
                await _fieldBLL.AddAsync(configData, token);
            }
        }

        return entity;
    }
}