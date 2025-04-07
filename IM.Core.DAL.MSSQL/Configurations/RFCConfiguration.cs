using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class RFCConfiguration : RfcConfigurationBase
    {
        protected override string ServiceForeignKey => "FK_RFC_Service";
        protected override string PriorityForeignKey => "FK_RFC_Priority";
        protected override string TypeForeignKey => "FK_RFC_RFCType";
        protected override string FormValuesForeignKey => "FK_RFC_FormValues";
        protected override string UrgencyForeignKey => "FK_RFC_Urgency";
        protected override string InfluenceForeignKey => "FK_RFC_Influence";
        protected override string OwnerForeignKey => "FK_RFC_Owner";
        protected override string InitiatorForeignKey => "FK_RFC_Initiator";
        protected override string FormForeignKey => "FK_RFC_Form";

        protected override void ConfigureDatabase(EntityTypeBuilder<ChangeRequest> builder)
        {
            builder.ToTable("RFC", "dbo");
            builder.HasKey(x => x.IMObjID);

            ConfigureColumns(builder);
        }

        private void ConfigureColumns(EntityTypeBuilder<ChangeRequest> builder)
        {
            builder.Property(x => x.IMObjID)
                .HasColumnName("ID");

            builder.Property(x => x.Number)
                .HasColumnName("Number");

            builder.Property(x => x.UrgencyID)
                .HasColumnName("UrgencyID");

            builder.Property(x => x.InfluenceID)
                .HasColumnName("InfluenceID");

            builder.Property(x => x.ServiceName)
                .HasColumnName("ServiceName")
                .HasMaxLength(250);

            builder.Property(x => x.Summary)
                .HasColumnName("Summary");

            builder.Property(x => x.UtcDateDetected)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateDetected");

            builder.Property(x => x.UtcDatePromised)
                .HasColumnType("datetime")
                .HasColumnName("UtcDatePromised");

            builder.Property(x => x.UtcDateClosed)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateClosed");

            builder.Property(x => x.UtcDateSolved)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateSolved");

            builder.Property(x => x.UtcDateModified)
                .IsRequired()
                .HasColumnType("datetime")
                .HasColumnName("UtcDateModified");

            builder.Property(x => x.UtcDateStarted)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateStarted");

            builder.Property(x => x.HTMLDescription)
                .HasColumnName("HTMLDescription");

            builder.Property(x => x.Description)
                .HasColumnType("ntext")
                .HasColumnName("Description");

            builder.Property(x => x.Target)
                .HasColumnName("Target");

            builder.Property(x => x.InitiatorID)
                .HasColumnName("InitiatorID");

            builder.Property(x => x.OwnerID)
                .HasColumnName("OwnerID");

            builder.Property(x => x.RowVersion)
                .IsRequired()
                .HasColumnType("timestamp")
                .IsRowVersion()
                .HasColumnName("RowVersion");

            builder.Property(x => x.ReasonObjectID)
                .HasColumnName("ReasonObjectID");

            builder.Property(x => x.ReasonObjectClassID)
                .HasColumnName("ReasonObjectClassID");

            builder.Property(x => x.FundingAmount)
                .HasColumnName("FundingAmount");

            builder.Property(x => x.RealizationDocumentID)
                .HasColumnName("RealizationDocumentID");

            builder.Property(x => x.RollbackDocumentID)
                .HasColumnName("RollbackDocumentID");

            builder.Property(x => x.CategoryID)
                .HasColumnName("CategoryID");

            builder.Property(x => x.ManhoursNormInMinutes)
                .HasColumnName("ManhoursNormInMinutes");

            builder.Property(x => x.EntityStateID)
                .HasColumnName("EntityStateID");

            builder.Property(x => x.EntityStateName)
                .HasColumnName("EntityStateName");

            builder.Property(x => x.WorkflowSchemeID)
                .HasColumnName("WorkflowSchemeID");

            builder.Property(x => x.WorkflowSchemeIdentifier)
                .HasColumnName("WorkflowSchemeIdentifier");

            builder.Property(x => x.WorkflowSchemeVersion)
                .HasColumnName("WorkflowSchemeVersion");

            builder.Property(x => x.OnWorkOrderExecutorControl)
                .HasColumnName("OnWorkOrderExecutorControl");

            builder.Property(x => x.InRealization)
                .HasColumnName("InRealization");

            builder.Property(x => x.QueueID)
                .HasColumnName("QueueID");

            builder.Property(x => x.RFCTypeID)
                .HasColumnName("RFCTypeID");

            builder.Property(x => x.ServiceID)
                .HasColumnName("ServiceID");

            builder.Property(x => x.PriorityID)
                .HasColumnName("PriorityID");

            builder.Property(x => x.ManhoursInMinutes)
                .HasColumnName("ManhoursInMinutes");

            builder.Property(x => x.FormValuesID).HasColumnName("FormValuesID");
            builder.Property(x => x.FormID).HasColumnName("FormID");
        }
    }
}
