using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class RuleConfiguration : RuleConfigurationBase
{
    protected override string PrimaryKey => "PK_Rule";

    protected override string ServiceTemplateForeignKey => "FK_Rule_ServiceTemplate";

    protected override string ServiceTemplateIDIndex => "IX_Rule_ServiceTemplateID";

    protected override string SLAIDIndex => "IX_Rule_SLAID";

    protected override string NameSLAIDUniqueIndex => "UI_RuleName_SLAID";

    protected override string NameOLAIDUniqueIndex => "UI_RuleName_OLAID";

    protected override void ConfigureDatabase(EntityTypeBuilder<Rule> builder)
    {
        builder.ToTable("Rule", Options.Scheme);

        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Value).HasColumnType("image").HasColumnName("Value");
        builder.Property(x => x.Note).HasColumnType("text").HasColumnName("Note");
        builder.Property(x => x.ServiceLevelAgreementID).HasColumnName("SLAID");
        builder.Property(x => x.OperationalLevelAgreementID).HasColumnName("OperationalLevelAgreementID");

        builder.Property(x => x.RowVersion)
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
