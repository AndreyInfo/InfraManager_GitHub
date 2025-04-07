using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;

namespace InfraManager.BLL.FieldEdit
{
    public interface IEntityEditorProvider
    {
        IEntityEditor GetEntityEditor(ObjectClassModel classModel);
    }
}
