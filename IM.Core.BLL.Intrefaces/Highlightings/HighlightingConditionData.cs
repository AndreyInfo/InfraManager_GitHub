using InfraManager.DAL.Highlightings;
using System;

namespace InfraManager.BLL.Highlighting;

public class HighlightingConditionData
{
    public Guid HighlightingID { get; set; }
    public ObjectClass? DirectoryParameter { get; init; }
    public HighlightingParameterEnum? EnumParameter { get; init; }
    public ConditionEnum Condition { get; init; }
    public Guid[] GuidValues { get; init; }
    public string? StringValue { get; init; }
    public int? IntValue1 { get; init; }
    public int? IntValue2 { get; init; }
    public string BackgroundColor { get; init; }
    public string FontColor { get; init; }
}
