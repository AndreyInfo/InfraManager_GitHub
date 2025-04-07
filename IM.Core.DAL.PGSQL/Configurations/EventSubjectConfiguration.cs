using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Events;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public class EventSubjectConfiguration : IEntityTypeConfiguration<EventSubject>
    {
        public void Configure(EntityTypeBuilder<EventSubject> entity)
        {
            entity.ToTable("event_subject", Options.Scheme);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.ClassId).HasColumnName("class_id");

            entity.Property(e => e.ObjectId).HasColumnName("object_id");

            entity.Property(e => e.SubjectName)
                .HasColumnName("subject_name")
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.SubjectValue)
                .HasColumnName("subject_value")
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.EventID)
                .HasColumnName("event_id")
                .IsRequired()
                .HasMaxLength(255);

            entity.HasMany(x => x.EventSubjectParam)
                .WithOne()
                .HasForeignKey("event_subject_id")
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_subject_param_event_subject");
        }
    }
}