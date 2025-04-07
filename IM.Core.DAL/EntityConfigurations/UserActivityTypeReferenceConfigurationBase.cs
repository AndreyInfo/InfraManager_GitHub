using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class UserActivityTypeReferenceConfigurationBase : IEntityTypeConfiguration<UserActivityTypeReference>
{
    protected abstract string PK_KEY { get; }
    protected abstract string Constraint_Type_Reference { get; }
    protected abstract string UI_UserActivityTypeID_ObjectID { get; }

    public void Configure(EntityTypeBuilder<UserActivityTypeReference> builder)
    {
        builder.HasKey(x => x.ID).HasName(PK_KEY);
        
        builder.HasIndex(x => new { x.ObjectID, x.UserActivityTypeID }, UI_UserActivityTypeID_ObjectID).IsUnique();

        builder.HasOne(x => x.Type)
            .WithMany(x => x.References)
            .HasConstraintName(Constraint_Type_Reference);

        DatabaseConfigure(builder);
    }
    
    protected abstract void DatabaseConfigure(EntityTypeBuilder<UserActivityTypeReference> builder);
    
}