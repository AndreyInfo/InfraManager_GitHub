using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class UserConfigurationBase : IEntityTypeConfiguration<User>
{
    protected abstract string KeyName { get; }
    protected abstract string LoginNameUI { get; }
    protected abstract string EmailUI { get; }
    protected abstract string WorkplaceFK { get; }
    protected abstract string PositionFK { get; }
    protected abstract string SIDDefaultValue { get; }
    protected abstract string IMObjIDDefaultValue { get; }
    protected abstract string ExternalIDDefaultValue { get; }
    protected abstract string DefaultIDValue { get; }
    
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.ID).HasName(KeyName);
        builder.Property(x => x.ID).HasDefaultValueSql(DefaultIDValue);

        builder.HasIndex(x => x.LoginName, LoginNameUI).IsUnique();
        builder.HasIndex(x => x.Email, EmailUI).IsUnique();

        builder.Ignore(e => e.FullName);

        builder.Property(e => e.IMObjID).HasDefaultValueSql(IMObjIDDefaultValue);
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.Email).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.Surname).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.Patronymic).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.SDWebPassword).HasMaxLength(250).IsRequired(false);
        builder.Property(e => e.TimeZoneID).HasMaxLength(250).IsRequired(false);
        builder.Property(e => e.Initials).HasMaxLength(30).IsRequired(false);
        builder.Property(e => e.Pager).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.Note).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.Phone).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.Phone1).HasMaxLength(15).IsRequired(false);
        builder.Property(e => e.Phone2).HasMaxLength(15).IsRequired(false);
        builder.Property(e => e.Phone3).HasMaxLength(15).IsRequired(false);
        builder.Property(e => e.Phone4).HasMaxLength(15).IsRequired(false);
        builder.Property(e => e.Fax).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.LoginName).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.Number).HasMaxLength(100).IsRequired(false);
        builder.Property(e => e.ExternalID).HasMaxLength(250).IsRequired(true)
            .HasDefaultValueSql(ExternalIDDefaultValue);
        builder.Property(e => e.SID).HasMaxLength(255).IsRequired(false)
            .HasDefaultValueSql(SIDDefaultValue);

        builder.HasOne(u => u.Workplace)
               .WithMany()
               .HasForeignKey(x => x.WorkplaceID)
               .HasConstraintName(WorkplaceFK);

        builder.HasOne(u => u.Position)
            .WithMany()
            .HasForeignKey(x => x.PositionID)
            .HasConstraintName(PositionFK);
        
        //TODO добавить FK
        builder.HasOne(u => u.Subdivision)
            .WithMany(sd => sd.Users)
            .HasForeignKey(u => u.SubdivisionID);

        //TODO добавить FK
        builder.HasMany(u => u.UserRoles)
            .WithOne()
            .HasForeignKey(ur => ur.UserID)
            .HasPrincipalKey(u => u.IMObjID);

        builder.HasOne(u => u.Manager)
            .WithMany()
            .HasForeignKey(x => x.ManagerID)
            .HasPrincipalKey(x=>x.IMObjID);

        ConfigureDataBase(builder);
    }

    protected abstract void ConfigureDataBase(EntityTypeBuilder<User> builder);
}
