using InfraManager.DAL.Highlightings;
using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Highlighting;

public class HighlightingConditionDetails
{
    public Guid ID { get; init; }
    public Guid HighlightingID { get; init; }
    public ObjectClass? DirectoryParameter { get; init; }
    public HighlightingParameterEnum? EnumParameter { get; init; }
    public ConditionEnum Condition { get; init; }
    public List<GuidDetail> GuidValues { get; set; }
    public string? StringValue { get; set; }
    public int? IntValue1 { get; set; }
    public int? IntValue2 { get; set; }
    public string BackgroundColor { get; init; }
    public string FontColor { get; init; }
}

public class GuidDetail
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
