using AutoMapper;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;

namespace InfraManager.BLL.Software.SoftwareModelUsingTypes;

public class SoftwareModelUsingTypeProfile : Profile
{
	public SoftwareModelUsingTypeProfile()
	{
		CreateMap<SoftwareModelUsingType, SoftwareModelUsingTypeDetails>();
	}
}
