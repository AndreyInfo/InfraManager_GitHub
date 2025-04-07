using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class StorageLocationConfigurationBase : IEntityTypeConfiguration<StorageLocation>
{
    protected abstract string KeyName { get; }
    protected abstract string StorageLocationReferancesFK { get; }

    public void Configure(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.HasKey(c => c.ID).HasName(KeyName);

        builder.Property(c => c.ID).ValueGeneratedOnAdd();
        builder.Property(c => c.Name).HasMaxLength(250).IsRequired(true);
        builder.Property(c => c.ExternalID).HasMaxLength(250).IsRequired(true);

        //TODO добавить FK
        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserID)
            .HasPrincipalKey(c=> c.IMObjID);


        builder.HasMany(c => c.StorageLocationReferences)
            .WithOne()
            .HasForeignKey(c => c.StorageLocationID)
            .HasConstraintName(StorageLocationReferancesFK);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<StorageLocation> builder);
}
