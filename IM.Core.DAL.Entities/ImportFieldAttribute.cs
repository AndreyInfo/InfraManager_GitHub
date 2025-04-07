using System;

namespace IM.Core.Import.BLL.Interface.Import.Models.SaveOrUpdateData;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ImportFieldAttribute:Attribute
{
    public string Name { get; set; }
    
    public bool Required { get; set; }
}