using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Report
{
    public class ReportFolderProfile : Profile
    {
        public ReportFolderProfile()
        {
            CreateMap<ReportFolderData, ReportFolder>()
                .ForMember(x => x.RowVersion, opt => opt.Ignore())
                .ForMember(x => x.ID, opt => opt.Ignore());

            CreateMap<ReportFolder, ReportFolderDetails>()
                .ReverseMap();

            CreateMap<ReportFolder, ReportFolder>();
        }
    }
}
