using AutoMapper;
using InfraManager.DAL.FormBuilder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder.Contracts;

namespace InfraManager.BLL.FormBuilder
{
    public class FormBuilderFormFieldProfile : Profile
    {
        public FormBuilderFormFieldProfile()
        {
            CreateMap<FormField, FormBuilderFormTabFieldDetails>()
                .ForMember(dst => dst.SpecialFields, m => m.MapFrom(src => JsonConvert.DeserializeObject<dynamic>(src.SpecialFields)));

            CreateMap<FormBuilderFormTabFieldDetails, FormField>()
                .ForMember(dst => dst.SpecialFields, m => m.MapFrom(src => src.SpecialFields));

        }
    }
}
