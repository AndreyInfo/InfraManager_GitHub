using AutoMapper;
using InfraManager.BLL.Settings;
using InfraManager.Core.Helpers;
using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.FormBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.FormBuilder.Contracts;

namespace InfraManager.BLL.FormBuilder
{
    public class FormBuilderFormProfile : Profile
    {
        public FormBuilderFormProfile()
        {
            CreateMap<Form, FormBuilderFormDetails>()
                .ForMember(dst => dst.StatusCode, m => m.MapFrom(src => src.Status))
                .ForMember(dst => dst.Status, m => m.MapFrom<LocalizedEnumResolver<Form, FormBuilderFormDetails, FormBuilderFormStatus>, FormBuilderFormStatus>(
                    queryItem => queryItem.Status));

            CreateMap<FormBuilderFullFormDetails, Form>()
                .ForMember(dst => dst.FormTabs, m => m.MapFrom(src => src.Elements.Select(s => s.Tab).ToList()));
        }
    }
}
