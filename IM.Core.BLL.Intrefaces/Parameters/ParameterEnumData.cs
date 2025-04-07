using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;

namespace InfraManager.BLL.Parameters
{
    [EntityCompare((int)ObjectClass.ParameterEnum, "Справочник параметров",
        AddOperationID = (int)OperationID.ParameterEnum_Add,
        EditOperationID = (int)OperationID.ParameterEnum_Update,
        DeleteOperationID = (int)OperationID.ParameterEnum_Delete)]
    public class ParameterEnumDetails
    {
        public Guid ID { get; init; }
        [FieldCompare("Название", 2)]
        public string Name { get; init; }
        public byte[] RowVersion { get; init; }
        [FieldCompare("Дерево", 3)]
        public bool IsTree { get; init; }
    }
}
