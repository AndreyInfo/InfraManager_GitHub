using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Events;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class EventSubjectConfiguration : IEntityTypeConfiguration<EventSubject>
    {
        public void Configure(EntityTypeBuilder<EventSubject> entity)
        {
            entity.ToTable("EventSubject", "dbo");
            entity.HasIndex(e => e.ObjectId, "IX_EventSubject_ObjectID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");

            entity.Property(e => e.ObjectId).HasColumnName("ObjectID");

            entity.Property(e => e.SubjectName)
                .HasColumnName("SubjectName")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.SubjectValue)
                .HasColumnName("SubjectValue")
                .IsRequired()
                .HasMaxLength(255);

            entity.HasMany(x => x.EventSubjectParam)
                .WithOne()
                .HasForeignKey("EventSubjectID")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventSubjectParam_EventSubject");
        }
    }
}
