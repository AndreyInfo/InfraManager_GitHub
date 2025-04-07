using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class SoftwareLicenceConfigurationBase : IEntityTypeConfiguration<SoftwareLicence>
{
    protected abstract string PrimaryKeyName { get; }

    protected abstract string ParentForeignKey { get; }
    protected abstract string ProductCatalogTypeForeignKey { get; }
    protected abstract string SoftwareModelForeignKey { get; }

    protected abstract string IndexByRoomIntID { get; }
    protected abstract string IndexBySoftwareModelID { get; }
    protected abstract string IndexByProductCatalogTypeID { get; }
    protected abstract string IndexBySoftwareLicenceModelID { get; }

    public void Configure(EntityTypeBuilder<SoftwareLicence> builder)
    {
        builder.HasKey(c=> c.ID).HasName(PrimaryKeyName);

        builder.HasIndex(e => e.RoomIntID, IndexByRoomIntID);
        builder.HasIndex(e => e.SoftwareModelID, IndexBySoftwareModelID);
        builder.HasIndex(e => e.ProductCatalogTypeID, IndexByProductCatalogTypeID);
        builder.HasIndex(e => e.SoftwareLicenceModelID, IndexBySoftwareLicenceModelID);


        builder.Property(e => e.ID).ValueGeneratedNever().HasComment("Идентификатор лицензии ПО");
        builder.Property(e => e.BeginDate).HasComment("Дата начала действия лицензии ПО");
        builder.Property(e => e.EndDate).HasComment("Дата окончания действия лицензии ПО");
        builder.Property(e => e.HaspadapterID).HasComment("Ссылка на адаптер, если это лицензия на HASP");
        builder.Property(e => e.ParentID).HasComment("Ссылка на родительскую лицензию ПО");
        builder.Property(e => e.RoomIntID).HasComment("Идентификатор комнаты");

        builder.Property(e => e.InventoryNumber)
            .IsRequired(true)
            .HasMaxLength(255);

        builder.Property(e => e.Name)
            .IsRequired(true)
            .HasMaxLength(250)
            .HasComment("Название лицензии ПО");

        builder.Property(e => e.Note)
            .IsRequired(true)
            .HasMaxLength(1000)
            .HasComment("Описание лицензии ПО");


        builder.HasOne(d => d.Parent)
            .WithMany(p => p.InverseParent)
            .HasForeignKey(d => d.ParentID)
            .HasConstraintName(ParentForeignKey);

        builder.HasOne(d => d.ProductCatalogType)
            .WithMany(p => p.SoftwareLicence)
            .HasForeignKey(d => d.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeForeignKey);

        builder.HasOne(d => d.SoftwareModel)
            .WithMany(p => p.SoftwareLicence)
            .HasForeignKey(d => d.SoftwareModelID)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName(SoftwareModelForeignKey);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<SoftwareLicence> builder);
}
