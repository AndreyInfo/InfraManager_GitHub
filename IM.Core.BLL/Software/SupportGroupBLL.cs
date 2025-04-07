using AutoMapper;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software;
public class SupportGroupBLL : ISupportGroupBLL, ISelfRegisteredService<ISupportGroupBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<SupportLineResponsible> _supportLineRepository;

    public SupportGroupBLL(IMapper mapper, IRepository<SupportLineResponsible> supportLineRepository)
    {
        _mapper = mapper;
        _supportLineRepository = supportLineRepository;
    }

    public void SaveSupportGroupForSoftwareModel(Guid softwareModelID, SoftwareModelData data)
    {
        if (data.GroupQueueID.HasValue)
        {
            var supportGroupData = GetSupportGroupDataForSoftwareModel(softwareModelID, (Guid)data.GroupQueueID);

            var supportGroup = _mapper.Map<SupportLineResponsible>(supportGroupData);

            _supportLineRepository.Insert(supportGroup);
        }
    }

    public async Task UpdateSupportGroupForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken)
    {
        var oldSupportGroup = await _supportLineRepository
            .FirstOrDefaultAsync(x => x.ObjectID == softwareModelID, cancellationToken);

        if (data.GroupQueueID.HasValue)
        {
            var supportGroupData = GetSupportGroupDataForSoftwareModel(softwareModelID, (Guid)data.GroupQueueID);

            if (oldSupportGroup == null)
            {
                SaveSupportGroupForSoftwareModel(softwareModelID, data);
            }
            else
            {
                _mapper.Map(supportGroupData, oldSupportGroup);
            }
        }
        else if (oldSupportGroup is not null)
        {
            _supportLineRepository.Delete(oldSupportGroup);
        }
    }

    private SupportLineResponsibleData GetSupportGroupDataForSoftwareModel(Guid softwareModelID, Guid GroupQueueID)
    {
        byte firstSupportLine = 1;

        return new SupportLineResponsibleData
        {
            ObjectID = softwareModelID,
            LineNumber = firstSupportLine,
            ObjectClassID = ObjectClass.SoftwareModel,
            OrganizationItemID = GroupQueueID,
            OrganizationItemClassID = ObjectClass.Group
        };
    }
}
