using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class RuleConfigurationBase : IEntityTypeConfiguration<Rule>
{
    protected abstract string PrimaryKey { get; }
    protected abstract string ServiceTemplateForeignKey { get; }
    protected abstract string ServiceTemplateIDIndex { get; }
    protected abstract string SLAIDIndex{ get; }
    protected abstract string NameSLAIDUniqueIndex{ get; }
    protected abstract string NameOLAIDUniqueIndex{ get; }
    public void Configure(EntityTypeBuilder<Rule> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKey);

        builder.HasIndex(x => x.ServiceTemplateID, ServiceTemplateIDIndex);
        builder.HasIndex(x => x.ServiceLevelAgreementID, SLAIDIndex);
        builder.HasIndex(x => new { x.Name, x.ServiceLevelAgreementID }, NameSLAIDUniqueIndex).IsUnique();
        builder.HasIndex(x => new { x.Name, x.OperationalLevelAgreementID }, NameOLAIDUniqueIndex).IsUnique();

        builder.Property(x => x.Name).HasMaxLength(250).IsRequired(true);
        builder.Property(x => x.Note).IsRequired(true);

        builder.HasOne(x => x.ServiceTemplate)
            .WithMany(x => x.Rules)
            .HasForeignKey(x => x.ServiceTemplateID)
            .HasConstraintName(ServiceTemplateForeignKey);

        ConfigureDatabase(builder);
    }
    
    protected abstract void ConfigureDatabase(EntityTypeBuilder<Rule> builder);
}