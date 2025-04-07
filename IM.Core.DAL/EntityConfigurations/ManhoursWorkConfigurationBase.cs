using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ManhoursWorkConfigurationBase : IEntityTypeConfiguration<ManhoursWork>
    {
        public void Configure(EntityTypeBuilder<ManhoursWork> builder)
        {
            builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

            builder.Property(x => x.IMObjID).ValueGeneratedNever();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(ManhoursWork.MaxDescriptionLength);
            builder.Property(x => x.Number).ValueGeneratedOnAdd();

            builder.HasMany(x => x.Entries)
                .WithOne()
                .HasForeignKey(x => x.WorkID);

            builder.Property(x => x.ExecutorID).ValueGeneratedNever();
            builder.HasOne(x => x.Executor)
                .WithMany()
                .HasForeignKey(x => x.ExecutorID)
                .HasConstraintName(ExecutorForeignKeyName)
                .HasPrincipalKey(x => x.IMObjID);

            builder.Property(x => x.InitiatorID).ValueGeneratedNever();
            builder.HasOne(x => x.Initiator)
                .WithMany()
                .HasForeignKey(x => x.InitiatorID)
                .HasConstraintName(InitiatorForeignKeyName)
                .HasPrincipalKey(x => x.IMObjID);

            builder.HasOne(x => x.UserActivityType)
                .WithMany()
                .HasForeignKey(x => x.UserActivityTypeID)
                .HasConstraintName(UserActivityTypeForeignKeyName)
                .HasPrincipalKey(x => x.ID);

            ConfigureDatabase(builder);
        }

        protected abstract string UserActivityTypeForeignKeyName { get; }

        protected abstract string ExecutorForeignKeyName { get; }

        protected abstract string InitiatorForeignKeyName { get; }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ManhoursWork> builder);
    }
}