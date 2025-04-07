using InfraManager.DAL.FormBuilder;
using System;

namespace InfraManager.DAL.ServiceCatalogue
{
    public interface IFormBuilder
    {
        Form Form { get; }
    }
}