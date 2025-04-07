using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk.Problems;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public class ProblemListItemMappingProfile : Profile
    {
        public ProblemListItemMappingProfile()
        {
            CreateMap<ProblemListQueryResultItem, ProblemListItem>()
                .ForMember(
                    m => m.ManhoursInMinutes,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<ProblemListQueryResultItem, ProblemListItem>,
                            int>(
                            item => item.Manhours))
                .ForMember(
                    m => m.ManhoursNormInMinutes, 
                     mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<ProblemListQueryResultItem, ProblemListItem>,
                            int>(
                            item => item.ManhoursNorm));
        }
    }
}
