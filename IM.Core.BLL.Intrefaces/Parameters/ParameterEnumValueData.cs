using InfraManager.CrossPlatform.WebApi.Contracts.Common.Attributes;
using System;

namespace InfraManager.BLL.Parameters
{
    [EntityCompare((int)ObjectClass.ParameterEnum, "Справочник параметров (значения)",
        AddOperationID = (int)OperationID.ParameterEnum_Add,
        EditOperationID = (int)OperationID.ParameterEnum_Update,
        DeleteOperationID = (int)OperationID.ParameterEnum_Delete)]
    public class ParameterEnumValueData
    {
        public Guid ID { get; init; }
        [FieldCompare("Параметр", 2)]
        public Guid ParameterEnumID { get; init; }
        [FieldCompare("Сортировка", 3)]
        public int OrderPosition { get; init; }
        [FieldCompare("Значение", 4)]
        public string Value { get; init; }
        [FieldCompare("Родительский элемент", 5)]
        public Guid? ParentID { get; set; }
        public bool HasChild { get; init; }
    }
}
