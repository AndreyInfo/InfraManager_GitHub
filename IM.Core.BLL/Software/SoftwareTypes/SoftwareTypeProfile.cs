using AutoMapper;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using SoftwareTypeEntity = InfraManager.DAL.Software.SoftwareType;

namespace InfraManager.BLL.Software.SoftwareTypes;

public class SoftwareTypeProfile : Profile
{
	public SoftwareTypeProfile()
	{
		CreateMap<SoftwareTypeEntity, SoftwareTypeDetails>();
	}
}
