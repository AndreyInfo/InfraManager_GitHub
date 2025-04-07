using InfraManager.DAL.FormBuilder;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class RfcConfigurationBase : IEntityTypeConfiguration<ChangeRequest>
    {
        public void Configure(EntityTypeBuilder<ChangeRequest> builder)
        {
            ConfigureDatabase(builder);

            ConfigureNavigationProperties(builder);

            builder.Property(x => x.Number)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Summary)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.UtcDateDetected)
                .IsRequired();

            builder.Property(x => x.HTMLDescription)
                .IsRequired();

            builder.Property(x => x.Target)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasMany(x => x.Manhours)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder.Property(x => x.ManhoursNormInMinutes)
                .IsRequired();

            builder.Property(x => x.EntityStateID)
                .HasMaxLength(50);

            builder.Property(x => x.EntityStateName)
                .HasMaxLength(250);

            builder.Property(x => x.WorkflowSchemeIdentifier)
                .HasMaxLength(50);

            builder.Property(x => x.WorkflowSchemeVersion)
                .HasMaxLength(50);

            builder.Property(x => x.OnWorkOrderExecutorControl)
                .IsRequired();

            builder.Property(x => x.InRealization)
                .IsRequired();

            builder.Property(x => x.RFCTypeID)
                .IsRequired();

            builder.Property(x => x.PriorityID)
                .IsRequired();
            builder.Ignore(x => x.TargetEntityStateID);

        }

        private void ConfigureNavigationProperties(EntityTypeBuilder<ChangeRequest> builder)
        {
            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryID);

            builder.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceID)
                .HasConstraintName(ServiceForeignKey);

            builder.HasOne(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.PriorityID)
                .HasConstraintName(PriorityForeignKey);

            builder.HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.RFCTypeID)
                .HasConstraintName(TypeForeignKey);

            builder
                .HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.QueueID);

            builder
                .HasOne(x => x.FormValues)
                .WithMany()
                .HasForeignKey(x => new { x.FormValuesID, x.FormID, })
                .HasPrincipalKey(x => new { FormValuesID = x.ID, FormID = x.FormBuilderFormID, })
                .HasConstraintName(FormValuesForeignKey);
            
            builder
                .HasMany(x => x.WorkOrderReferences)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder.HasOne(x => x.Urgency)
                .WithMany()
                .HasForeignKey(x => x.UrgencyID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(UrgencyForeignKey)
                .IsRequired(false);

            builder.HasOne(x => x.Influence)
                .WithMany()
                .HasForeignKey(x => x.InfluenceID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(InfluenceForeignKey)
                .IsRequired(false);

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(OwnerForeignKey)
                .IsRequired(false);

            builder.HasOne(x => x.Initiator)
                .WithMany()
                .HasForeignKey(x => x.InitiatorID)
                .HasPrincipalKey(x => x.IMObjID)
                // .HasConstraintName(InitiatorForeignKey)
                .IsRequired(false);

            builder.HasOne<Form>().WithMany().HasForeignKey(x => x.FormID).HasConstraintName(FormForeignKey);
        }

        protected abstract string ServiceForeignKey { get; }
        protected abstract string PriorityForeignKey { get; }
        protected abstract string TypeForeignKey { get; }
        protected abstract string FormValuesForeignKey { get; }
        protected abstract string UrgencyForeignKey { get; }
        protected abstract string InfluenceForeignKey { get; }
        protected abstract string OwnerForeignKey { get; }
        protected abstract string InitiatorForeignKey { get; }
        protected abstract string FormForeignKey { get; }
        
        protected abstract void ConfigureDatabase(EntityTypeBuilder<ChangeRequest> builder);
    }
}
