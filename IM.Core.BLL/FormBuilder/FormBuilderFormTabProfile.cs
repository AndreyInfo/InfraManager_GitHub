using AutoMapper;
using InfraManager.DAL.FormBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder.Contracts;

namespace InfraManager.BLL.FormBuilder
{
    public class FormBuilderFormTabProfile : Profile
    {
        public FormBuilderFormTabProfile()
        {
            CreateMap<FormTab, FormBuilderFormTabDetails>()
                .ReverseMap();
        }
    }
}
