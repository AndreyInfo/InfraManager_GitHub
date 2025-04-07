using AutoMapper;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.DAL.FormBuilder;

namespace InfraManager.BLL.FormBuilder
{
    public class FormBuilderDynamicOptionsProfile : Profile
    {
        public FormBuilderDynamicOptionsProfile()
        {
            CreateMap<DynamicOptions, DynamicOptionsDetails>()
               .ReverseMap();
        }
    }
}
