using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class UserActivityTypeReferenceConfiguration : UserActivityTypeReferenceConfigurationBase
{
    protected override string PK_KEY => "pk_user_activity_type_reference";
    protected override string Constraint_Type_Reference => "fk_user_activity_type_reference_user_activity_type";
    protected override string UI_UserActivityTypeID_ObjectID => "ui_user_activity_type_user";

    protected override void DatabaseConfigure(EntityTypeBuilder<UserActivityTypeReference> builder)
    {
        builder.ToTable("user_activity_type_reference", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(x => x.UserActivityTypeID).HasColumnName("user_activity_type_id");
        builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id");
        builder.Property(x => x.ObjectID).HasColumnName("object_id");
        builder.Property(x => x.ReferenceClassID).HasColumnName("reference_class_id");
        builder.Property(e => e.ReferenceObjectID).HasColumnName("reference_object_id");
    }
}