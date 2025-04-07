using AutoMapper;
using InfraManager.BLL.ServiceDesk.FormDataValue;
using InfraManager.DAL.ServiceDesk;
using InfraManager.WebApi.Contracts.Models.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls;

public class FormValuesDataProfile : Profile
{
    public FormValuesDataProfile()
    {
        CreateMap<FormValuesData, FormValues>().ConvertUsing<FormValuesDataConverter>();
        CreateMap<FormValues, FormValuesDetailsModel>().ConvertUsing<FormValuesConverter>();
    }
}