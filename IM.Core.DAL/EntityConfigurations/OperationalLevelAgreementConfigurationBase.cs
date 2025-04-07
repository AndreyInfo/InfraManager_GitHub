using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class OperationalLevelAgreementConfigurationBase : IEntityTypeConfiguration<OperationalLevelAgreement>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UniqueIndexName { get; }
    protected abstract string FormForeignKey { get; }
    
    public void Configure(EntityTypeBuilder<OperationalLevelAgreement> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
        builder.Property(x => x.IMObjID).ValueGeneratedOnAdd();
        builder.HasIndex(x => x.Name, UniqueIndexName).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(OperationalLevelAgreement.MaxNameLength).IsRequired(true);
        builder.Property(x => x.Number).HasMaxLength(OperationalLevelAgreement.MaxNumberLength).IsRequired(true);
        builder.Property(x => x.TimeZoneID).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.Note).HasMaxLength(OperationalLevelAgreement.MaxNoteLength).IsRequired(false);
        builder.Property(x => x.UtcFinishDate).IsRequired(false);
        builder.Property(x => x.UtcStartDate).IsRequired(false);

        builder.HasOne(x => x.Form)
            .WithMany()
            .HasForeignKey(x => x.FormID)
            .HasConstraintName(FormForeignKey)
            .IsRequired(false);

        builder.HasMany(x => x.Rules).WithOne().HasForeignKey(x => x.OperationalLevelAgreementID).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.ConcludedWith).WithOne().HasForeignKey(x => x.ID).HasPrincipalKey(x => x.IMObjID);
        
        ConfigureDataBase(builder);
    }
    
    protected abstract void ConfigureDataBase(EntityTypeBuilder<OperationalLevelAgreement> builder);
}