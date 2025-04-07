using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class UserActivityTypeReferenceConfiguration : UserActivityTypeReferenceConfigurationBase
{
    protected override string PK_KEY => "PK_UserActivityTypeReference";
    protected override string Constraint_Type_Reference => "FK_UserActivityTypeReference_UserActivityType";
    protected override string UI_UserActivityTypeID_ObjectID => "UI_User_Activity_Type_User";

    protected override void DatabaseConfigure(EntityTypeBuilder<UserActivityTypeReference> builder)
    {
        builder.ToTable("UserActivityTypeReference", "dbo");

        builder.Property(x => x.ID).HasDefaultValueSql("NEWID()").HasColumnName("ID");
        builder.Property(x => x.UserActivityTypeID).HasColumnName("UserActivityTypeID");
        builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(x => x.ObjectID).HasColumnName("ObjectID");
        builder.Property(x => x.ReferenceClassID).HasColumnName("ReferenceClassID");
        builder.Property(e => e.ReferenceObjectID).HasColumnName("ReferenceObjectID");
    }
}
