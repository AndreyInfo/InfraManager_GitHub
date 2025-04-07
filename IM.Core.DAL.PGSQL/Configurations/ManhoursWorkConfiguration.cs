using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.Manhours;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ManhoursWorkConfiguration : ManhoursWorkConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_manhours_work";

        protected override string UserActivityTypeForeignKeyName =>
            "fk_manhours_work_user_activity_type_id_user_activity_type";

        protected override string ExecutorForeignKeyName => "fk_manhours_work_executor_id_user";
        protected override string InitiatorForeignKeyName => "fk_manhours_work_initiator_id_user";

        protected override void ConfigureDatabase(EntityTypeBuilder<ManhoursWork> builder)
        {
            builder.ToTable("manhours_work", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("id");
            builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
            builder.Property(x => x.ObjectID).HasColumnName("object_id");
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.Number).HasColumnName("number");
            builder.Property(x => x.ExecutorID).HasColumnName("executor_id");
            builder.Property(x => x.InitiatorID).HasColumnName("initiator_id");
            builder.Property(x => x.UserActivityTypeID).HasColumnName("user_activity_type_id");
        }
    }
}