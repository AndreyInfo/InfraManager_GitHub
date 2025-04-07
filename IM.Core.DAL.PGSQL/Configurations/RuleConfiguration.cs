using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class RuleConfiguration : RuleConfigurationBase
{
    protected override string PrimaryKey => "pk_rule";

    protected override string ServiceTemplateForeignKey => "fk_rule_service_template";

    protected override string ServiceTemplateIDIndex => "ix_rule_service_template_id";

    protected override string SLAIDIndex => "ix_rule_sla_id";

    protected override string NameSLAIDUniqueIndex => "ui_rule_name_sla_id";

    protected override string NameOLAIDUniqueIndex => "ui_rule_name_ola_id";

    protected override void ConfigureDatabase(EntityTypeBuilder<Rule> builder)
    {
        builder.ToTable("rule", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.Sequence).HasColumnName("sequence");
        builder.Property(e => e.ServiceTemplateID).HasColumnName("service_template_id");
        builder.Property(e => e.ServiceLevelAgreementID).HasColumnName("sla_id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Value).HasColumnName("value").HasColumnType("image");
        builder.Property(x => x.Note).HasColumnName("note").HasColumnType("text");
        builder.Property(x => x.OperationalLevelAgreementID).HasColumnName("operational_level_agreement_id");
        
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}