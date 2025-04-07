using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class ManhoursWorkConfiguration : ManhoursWorkConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ManhoursWork";

        protected override string UserActivityTypeForeignKeyName =>
            "FK_ManhoursWork_UserActivityTypeId_UserActivityType";

        protected override string ExecutorForeignKeyName => "FK_ManhoursWork_ExecutorID_User";
        protected override string InitiatorForeignKeyName => "FK_ManhoursWork_InitiatorID_User";

        protected override void ConfigureDatabase(EntityTypeBuilder<ManhoursWork> builder)
        {
            builder.ToTable("ManhoursWork", "dbo");

            builder.Property(x => x.IMObjID).HasColumnName("ID");
            builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
            builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
            builder.Property(x => x.Description).HasColumnName("Description");
            builder.Property(x => x.Number).HasColumnName("Number").HasDefaultValueSql("NEXT VALUE FOR [dbo].[ManhoursWorkNumber]");
            builder.Property(x => x.ExecutorID).HasColumnName("ExecutorID");
            builder.Property(x => x.InitiatorID).HasColumnName("InitiatorID");
            builder.Property(x => x.UserActivityTypeID).HasColumnName("UserActivityTypeID");
        }
    }
}