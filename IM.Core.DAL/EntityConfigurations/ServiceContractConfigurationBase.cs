using System;
using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceContractConfigurationBase : IEntityTypeConfiguration<ServiceContract>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string LifeCycleStateForeignKeyName { get; }
    protected abstract string ProductCatalogTypeForeignKeyName { get; }
    protected abstract string SupplierForeignKeyName { get; }
    protected abstract string WorkOrderForeignKeyName { get; }
    protected abstract string FinanceCenterForeignKeyName { get; }
    protected abstract string ServiceContractModelForeignKeyName { get; }

    public void Configure(EntityTypeBuilder<ServiceContract> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Notice).IsRequired(true).HasMaxLength(255);
        builder.Property(x => x.ExternalNumber).IsRequired(true).HasMaxLength(250).HasDefaultValueSql("('')");
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(500).HasDefaultValueSql("('')");
        builder.Property(x => x.AddressLicence).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.LoginLicence).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.PasswordLicence).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.AddressAsset).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.LoginAsset).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.PasswordAsset).IsRequired(true).HasMaxLength(250);

        builder.Property(x => x.UpdateType).HasDefaultValueSql("(1)");
        builder.Property(x => x.UpdatePeriod).HasDefaultValueSql("(0)");
        builder.Property(x => x.NdsType).HasDefaultValueSql("(0)");
        builder.Property(x => x.NdsPercent).HasDefaultValueSql("(4)");

        //TODO добавить FK
        builder.HasOne(x => x.ServiceContractType)
            .WithMany()
            .HasForeignKey(c=> c.ServiceContractTypeID)
            .HasPrincipalKey(x => x.ID);

        builder.HasOne(x => x.LifeCycleState)
            .WithMany()
            .HasForeignKey(c=> c.LifeCycleStateID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(LifeCycleStateForeignKeyName);

        builder.HasOne(x => x.ProductCatalogType)
            .WithMany()
            .HasForeignKey(c => c.ProductCatalogTypeID)
            .HasPrincipalKey(x => x.IMObjID)
            .HasConstraintName(ProductCatalogTypeForeignKeyName);

        builder.HasOne(x => x.WorkOrder)
            .WithMany()
            .HasForeignKey(c=> c.WorkOrderID)
            .HasPrincipalKey(x => x.IMObjID)
            .HasConstraintName(WorkOrderForeignKeyName);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(c=> c.SupplierID)
            .HasConstraintName(SupplierForeignKeyName);

        builder.HasOne(x => x.FinanceCenter)
            .WithMany()
            .HasForeignKey(c => c.FinanceCenterID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(FinanceCenterForeignKeyName);

        builder.HasOne(x => x.Model)
            .WithMany()
            .HasForeignKey(x => x.ModelID)
            .HasPrincipalKey(x => x.IMObjID)
            .HasConstraintName(ServiceContractModelForeignKeyName);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<ServiceContract> builder);
}