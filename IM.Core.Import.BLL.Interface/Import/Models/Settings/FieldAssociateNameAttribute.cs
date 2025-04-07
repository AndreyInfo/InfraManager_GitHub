namespace IM.Core.Import.BLL.Interface.Import.Models.Settings;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FieldAssociateNameAttribute : Attribute
{
    public string FieldName { get; init; }

    public string ClassName { get; init; }
}