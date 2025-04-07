using InfraManager.DAL.AccessManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class RoleOperationConfiguration : IEntityTypeConfiguration<RoleOperation>
    {
        public void Configure(EntityTypeBuilder<RoleOperation> builder)
        {
            builder.ToTable("RoleOperation", "dbo");
            builder.HasKey(x => new { x.RoleID, x.OperationID });

            builder.HasOne(x => x.Operation)
                .WithMany(x => x.RoleOperations)
                .HasForeignKey(x => x.OperationID);
        }
    }
}
