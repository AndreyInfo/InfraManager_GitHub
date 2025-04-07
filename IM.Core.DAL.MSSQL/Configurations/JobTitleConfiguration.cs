using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class JobTitleConfiguration : JobTitleConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Должности";
    protected override string UniqueNameConstraint => "UK_JobTitle_Name";
    protected override string UniqueIMObjIDConstraint => "UK_JobTitle_IMObjID";
    protected override string DefaultValueIMObjID => "NEWID()";

    protected override void ConfigureDataBase(EntityTypeBuilder<JobTitle> builder)
    {
        builder.ToTable("Должности", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("Идентификатор");
        builder.Property(x => x.IMObjID).HasColumnName("IMObjID");
        builder.Property(x => x.Name).HasColumnName("Название");
        builder.Property(x => x.ComplementaryId).HasColumnName("ComplementaryID");
    }
}
