using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class JobTitleConfigurationBase : IEntityTypeConfiguration<JobTitle>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UniqueNameConstraint { get; }
    protected abstract string UniqueIMObjIDConstraint { get; }
    protected abstract string DefaultValueIMObjID { get; }

    public void Configure(EntityTypeBuilder<JobTitle> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.HasIndex(x => x.Name, UniqueNameConstraint).IsUnique();
        builder.HasIndex(x => x.IMObjID, UniqueIMObjIDConstraint).IsUnique();

        builder.Property(x => x.ID).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255);
        builder.Property(x => x.IMObjID).ValueGeneratedOnAdd().HasDefaultValueSql(DefaultValueIMObjID);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<JobTitle> builder);
}
