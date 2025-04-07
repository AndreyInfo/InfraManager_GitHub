using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.EntityConfigurations;
public abstract class ServiceTechnicalFailureCategoryBase : IEntityTypeConfiguration<ServiceTechnicalFailureCategory>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string ServiceIDColumnName { get; }
    protected abstract string CategoryIDColumnName { get; }
    protected abstract string ServiceCategoryUniqueKeyName { get; }
    protected abstract string ServiceForeignKeyName { get; }
    protected abstract string CategoryForeignKeyName { get; }
    protected abstract string GroupForeignKeyName { get; }
    protected abstract string IMObjIDDefaultValue { get; }

    public void Configure(EntityTypeBuilder<ServiceTechnicalFailureCategory> builder)
    {
        builder.HasKey(c => c.ID).HasName(PrimaryKeyName);
        
        builder.Property(c => c.ID).ValueGeneratedOnAdd();
        builder.Property(c => c.IMObjID).ValueGeneratedOnAdd().HasDefaultValueSql(IMObjIDDefaultValue);
        builder.Property<int>(CategoryIDColumnName);
        builder.Property<Guid>(ServiceIDColumnName);

        builder
            .HasIndex(ServiceIDColumnName, CategoryIDColumnName)
            .HasDatabaseName(ServiceCategoryUniqueKeyName)
            .IsUnique();

        builder
            .HasOne(c => c.Reference)
            .WithMany()
            .HasForeignKey(ServiceIDColumnName)
            .HasConstraintName(ServiceForeignKeyName)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        builder
            .HasOne<TechnicalFailureCategory>()
            .WithMany(x => x.Services)
            .HasForeignKey(CategoryIDColumnName)
            .HasConstraintName(CategoryForeignKeyName)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        builder.HasOne(c => c.Group)
            .WithMany()
            .HasForeignKey(c => c.GroupID)
            .HasConstraintName(GroupForeignKeyName)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<ServiceTechnicalFailureCategory> builder);
}
