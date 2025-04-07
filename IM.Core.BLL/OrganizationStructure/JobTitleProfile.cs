using AutoMapper;
using InfraManager.BLL.OrganizationStructure.JobTitles;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure;

internal class JobTitleProfile : Profile
{
    public JobTitleProfile()
    {
        CreateMap<JobTitle, JobTitleDetails>();
        CreateMap<JobTitleData, JobTitle>();
    }
}
