using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class SideOrganizationConfiguration : SideOrganizationConfigurationBase
{
    protected override string PrimaryKeyName => "PK_SideOrganization";

    protected override void ConfigureDatabase(EntityTypeBuilder<SideOrganization> builder)
    {
        builder.ToTable("SideOrganization", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name").IsUnicode(false);
        builder.Property(x => x.Phone).HasColumnName("Phone").IsUnicode(false);
        builder.Property(x => x.Fax).HasColumnName("Fax").IsUnicode(false);
        builder.Property(x => x.Mail).HasColumnName("Mail").IsUnicode(false);
        builder.Property(x => x.Note).HasColumnName("Note").IsUnicode(false);
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
    }
}