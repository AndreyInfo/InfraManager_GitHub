using AutoMapper;
using InfraManager.DAL;

namespace InfraManager.BLL.Report
{
    public class ReportProfile :Profile
    {
        public ReportProfile()
        {
            CreateMap<ReportDetails, Reports>()
                .ReverseMap();

            CreateMap<ReportWithPathDetails, ReportForTableDetails>();

            CreateMap<Reports, ReportWithPathDetails>();

            CreateMap<ReportData, Reports>();
        }
    }
}
