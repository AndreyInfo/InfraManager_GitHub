using AutoMapper;
using InfraManager.BLL.FormBuilder.Forms;
using InfraManager.DAL.FormBuilder;

namespace InfraManager.BLL.FormBuilder;

public class FormProfile : Profile
{
    public FormProfile()
    {
        CreateMap<FormBuilderFormData, Form>();
    }
}