using System;
using InfraManager;

namespace IM.Core.DM.BLL.Interfaces;

public class FieldConfiguration
{
    public string FieldName { get; set; }

    public Guid ClassID { get; set; }
    
    public string Value { get; set; }

    public ConcordanceObjectType? FieldEnum => Enum.TryParse(FieldName, out ConcordanceObjectType value) ? value : null;
}