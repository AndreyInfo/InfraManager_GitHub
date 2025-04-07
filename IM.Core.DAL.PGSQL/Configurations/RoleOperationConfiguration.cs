using IM.Core.DAL.Postgres;
using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class RoleOperationConfiguration : IEntityTypeConfiguration<RoleOperation>
    {
        public void Configure(EntityTypeBuilder<RoleOperation> builder)
        {
            builder.ToTable("role_operation", Options.Scheme);
            builder.HasKey(x => new {x.RoleID, x.OperationID});


            builder.Property(e => e.OperationID)
                .IsRequired()
                .HasColumnName("operation_id");

            builder.Property(e => e.RoleID)
                .IsRequired()
                .HasColumnName("role_id");


            builder.HasOne(x => x.Operation)
                .WithMany(x => x.RoleOperations)
                .HasForeignKey(x => x.OperationID);
        }
    }
}