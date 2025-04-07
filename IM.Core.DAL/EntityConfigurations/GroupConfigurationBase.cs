using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class GroupConfigurationBase : IEntityTypeConfiguration<Group>
{
    protected abstract string KeyName { get;  }
    protected abstract string DefaultValueID { get; }
    protected abstract string DefaultValueNote { get; }
    protected abstract string UsersFK { get; }
    protected abstract string UIName { get; }

    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(c => c.IMObjID).HasName(KeyName);

        builder.HasIndex(c => c.Name, UIName).IsUnique();

        builder.Property(c => c.IMObjID).HasDefaultValueSql(DefaultValueID);
        builder.Property(c => c.Name).HasMaxLength(250).IsRequired(true);

        builder.Property(c => c.Note)
            .HasDefaultValue(DefaultValueNote)
            .HasMaxLength(500)
            .IsRequired(true);

        builder.HasMany(x => x.QueueUsers)
            .WithOne()
            .HasForeignKey(q => q.GroupID)
            .HasConstraintName(UsersFK);

        // TODO добавить FK
        builder.HasOne(x => x.ResponsibleUser)
            .WithMany()
            .HasForeignKey(u => u.ResponsibleID)
            .HasPrincipalKey(x => x.IMObjID);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<Group> builder);
}
