using AutoMapper;
using InfraManager.BLL.Software.SoftwareModelRecognitions;
using InfraManager.BLL.Software.SoftwareModels;
using InfraManager.DAL;
using InfraManager.DAL.Software;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelTabs.Recognitions;
public class SoftwareModelRecognitionBLL : ISoftwareModelRecognitionBLL, ISelfRegisteredService<ISoftwareModelRecognitionBLL>
{
    private readonly IMapper _mapper;
    private readonly IRepository<SoftwareModelRecognition> _softwareModelRecognitionRepository;

    public SoftwareModelRecognitionBLL(IMapper mapper, IRepository<SoftwareModelRecognition> softwareModelRecognitionRepository)
    {
        _mapper = mapper;
        _softwareModelRecognitionRepository = softwareModelRecognitionRepository;
    }

    public void SaveModelRecognitionForSoftwareModel(Guid softwareModelID, SoftwareModelData data)
    {
        if (data.VersionRecognitionID.HasValue &&
            data.VersionRecognitionLvl.HasValue &&
            data.RedactionRecognition.HasValue)
        {
            var modelRecognition = new SoftwareModelRecognition
            {
                SoftwareModelID = softwareModelID,
                VersionRecognitionID = data.VersionRecognitionID.Value,
                VersionRecognitionLvl = data.VersionRecognitionLvl.Value,
                RedactionRecognition = data.RedactionRecognition.Value
            };

            _softwareModelRecognitionRepository.Insert(modelRecognition);
        }
    }

    public async Task UpdateModelRecognitionForSoftwareModelAsync(Guid softwareModelID, SoftwareModelData data, CancellationToken cancellationToken)
    {
        var oldModelRecognition = await _softwareModelRecognitionRepository
            .FirstOrDefaultAsync(x => x.SoftwareModelID == softwareModelID, cancellationToken);

        if (oldModelRecognition is null)
        {
            SaveModelRecognitionForSoftwareModel(softwareModelID, data);
        }
        else
        {
            var modelRecognition = new SoftwareModelRecognitionData
            {
                SoftwareModelID = softwareModelID,
                VersionRecognitionID = data.VersionRecognitionID ?? oldModelRecognition.VersionRecognitionID,
                VersionRecognitionLvl = data.VersionRecognitionLvl ?? oldModelRecognition.VersionRecognitionLvl,
                RedactionRecognition = data.RedactionRecognition ?? oldModelRecognition.RedactionRecognition
            };

            _mapper.Map(modelRecognition, oldModelRecognition);
        }
    }
}
