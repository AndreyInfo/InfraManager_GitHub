using AutoMapper;
using InfraManager.BLL.ServiceDesk.FormDataValue;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.WorkOrders;

public class FormValuesDataModelProfile : Profile
{
    public FormValuesDataModelProfile()
    {
        CreateMap<FormValuesDataModel, FormValuesData>();
        CreateMap<DataItemModel, DataItem>();
        CreateMap<DataItemTableRowModel, DataItemTableRow>();
    }
}