using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public partial class SideOrganizationConfiguration : SideOrganizationConfigurationBase
{
    protected override string PrimaryKeyName => "pk_side_organization";

    protected override void ConfigureDatabase(EntityTypeBuilder<SideOrganization> builder)
    {
        builder.ToTable("side_organization", Options.Scheme);
        
        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Phone).HasColumnName("phone");
        builder.Property(x => x.Fax).HasColumnName("fax");
        builder.Property(x => x.Mail).HasColumnName("mail");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.HasXminRowVersion(x => x.RowVersion);
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");

        OnConfigurePartial(builder);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<SideOrganization> entity);
}