using System;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.FormBuilder.Enums;

namespace InfraManager.BLL.FormBuilder;

public class FormBuilderFilter : BaseFilter
{
    public ObjectClass? ClassID { get; init; }
    public FormBuilderFormStatus[] Statuses { get; init; }
    public Guid? FormID { get; init; }
    public Guid? MainID { get; init; }
    public bool OnlyHighLevel { get; init; }
}