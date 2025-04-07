using InfraManager.DAL.FormBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FormBuilderFormFieldSettingsConfigurationBase : IEntityTypeConfiguration<FormFieldSettings>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string UserForeignKeyName { get; }
    protected abstract string FieldForeignKeyName { get; }
    protected abstract string UniqueKeyName { get; }

    public void Configure(EntityTypeBuilder<FormFieldSettings> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
        builder.HasIndex(x => new { x.UserID, x.FieldID, }).HasDatabaseName(UniqueKeyName);

        builder.Property(x => x.ID).ValueGeneratedOnAdd();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserID)
            .HasPrincipalKey(x => x.IMObjID)
            .HasConstraintName(UserForeignKeyName)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<FormField>()
            .WithMany()
            .HasForeignKey(x => x.FieldID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(FieldForeignKeyName)
            .OnDelete(DeleteBehavior.Cascade);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<FormFieldSettings> builder);
}