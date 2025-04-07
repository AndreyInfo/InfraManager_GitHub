using AutoMapper;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareModelRecognitions;

public class SoftwareModelRecognitionProfile : Profile
{
	public SoftwareModelRecognitionProfile()
	{
		CreateMap<SoftwareModelRecognitionData, SoftwareModelRecognition>();
	}
}
