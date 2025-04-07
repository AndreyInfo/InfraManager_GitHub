using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Problems
{
    public class ProblemCauseProfile : Profile
    {
        public ProblemCauseProfile()
        {
            CreateMap<ProblemCause, ProblemCauseDetails>();
            CreateMap<ProblemCauseData, ProblemCause>().IgnoreNulls();
        }
    }
}
