using System;

namespace InfraManager.BLL.ServiceCatalogue.Rules;

public class RuleData
{
    public string Name { get; init; }
    public string Note { get; init; }
    public int Sequence { get; init; }
    public Guid? SLAID { get; init; }
    public int? OperationalLevelAgreementID { get; init; }
    public Guid? ServiceTemplateID { get; init; }
    public byte[] RowVersion { get; init; }
}