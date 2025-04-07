using AutoMapper;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.BLL.FormBuilder.Forms;
using InfraManager.DAL.FormBuilder;

namespace InfraManager.BLL.FormBuilder;

public class FullFormProfile : Profile
{
    public FullFormProfile()
    {
        CreateMap<FormBuilderFormData, Form>();

        CreateMap<FormBuilderFormTabDetails, FormTab>();

        CreateMap<FormBuilderFormTabFieldDetails, FormField>();

        CreateMap<DynamicOptionsDetails, DynamicOptions>();

        CreateMap<Form, Form>()
            .ForMember(x => x.FormTabs, x => x.Condition(x => x.FormTabs != null));

        CreateMap<FormFieldSettings, FormBuilderFormFieldSettingDetails>();
    }
}