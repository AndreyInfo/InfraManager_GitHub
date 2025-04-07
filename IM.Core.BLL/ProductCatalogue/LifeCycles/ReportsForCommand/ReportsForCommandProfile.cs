using AutoMapper;
using InfraManager.BLL.ReportsForCommand;

namespace InfraManager.BLL.ProductCatalogue.LifeCycles.ReportsForCommand;

internal class ReportsForCommandProfile : Profile
{
    public ReportsForCommandProfile()
    {
        CreateMap<ReportForCommandData, ReportForCommandDetails>();
    }
}