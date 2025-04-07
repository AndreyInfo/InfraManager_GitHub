using IM.Core.DAL.Postgres;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public partial class SoftwareModelConfiguration : SoftwareModelConfigurationBase
    {
        protected override string KeyName => "pk_software_model";

        protected override void ConfigureDataBase(EntityTypeBuilder<SoftwareModel> builder)
        {
            builder.ToTable("software_model", Options.Scheme);

            builder.HasIndex(e => e.CommercialModelID, "ix_software_model_commercial_model_id");

            builder.HasIndex(e => e.IsCommercial, "ix_software_model_is_commercial");

            builder.HasIndex(e => e.ManufacturerID, "ix_software_model_manufacturer_id");

            builder.HasIndex(e => e.ParentID, "ix_software_model_parent_id");

            builder.HasIndex(e => e.Removed, "ix_software_model_true_id");

            builder.HasIndex(e => e.SoftwareModelUsingTypeID, "ix_software_model_software_type_id");

            builder.HasIndex(e => e.SoftwareTypeID, "ix_software_model_software_model_using_type_id");

            builder.HasIndex(e => e.TrueID, "ix_software_model_removed");


            builder.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .HasComment("Идентификатор модели ПО");

            builder.Property(e => e.SoftwareTypeID)
                .HasColumnName("software_type_id")
                .HasComment("Ссылка на тип ПО");

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasComment("Название модели ПО");

            builder.Property(e => e.Note)
                .HasColumnName("note")
                .HasComment("Описание модели ПО");

            builder.Property(e => e.Version)
                .HasColumnName("version")
                .HasComment("Версия модели ПО");

            builder.Property(e => e.Code)
                .HasColumnName("code")
                .HasComment("Код модели ПО");

            builder.Property(e => e.ManufacturerID)
                .HasColumnName("manufacturer_id")
                .HasComment("Идентификатор производителя модели ПО");

            builder.Property(e => e.SupportDate)
                .HasColumnName("support_date")
                .HasColumnType("timestamp(3)")
                .HasComment("Дата до которой производится поддержка данной модели ПО");

            builder.Property(e => e.Template)
                .HasColumnName("template")
                .HasComment("Шаблон модели ПО (справочник)");

            builder.Property(e => e.Removed)
                .HasColumnName("removed")
                .HasComment("Флаг удаления");

            builder.Property(e => e.ParentID)
                .HasColumnName("parent_id")
                .HasComment("Ссылка на родительскую модель ПО");

            builder.Property(e => e.TrueID)
                .HasColumnName("true_id")
                .HasComment("Ссылка на модель ПО, являющуюся синонимом данной");

            builder.Property(e => e.CreateDate)
                .HasColumnName("create_date")
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Дата создания модели ПО");

            builder.Property(e => e.SoftwareModelUsingTypeID)
                .HasColumnName("software_model_using_type_id");

            builder.Property(e => e.IsCommercial)
                .HasColumnName("is_commercial");

            builder.Property(e => e.CommercialModelID)
                .HasColumnName("commercial_model_id");

            builder.Property(e => e.ProcessNames)
                .HasColumnName("process_names");

            builder.Property(e => e.ExternalID)
                .HasColumnName("external_id");

            builder.Property(e => e.UtcDateCreated)
                .HasColumnName("utc_date_created")
                .HasColumnType("timestamp(3)");

            builder.Property(e => e.ModelRedaction)
                .HasColumnName("model_redaction");

            builder.Property(e => e.OwnerModelID)
                .HasColumnName("owner_model_id");

            builder.Property(e => e.OwnerModelClassID)
                .HasColumnName("owner_model_class_id");

            builder.Property(e => e.ModelDistribution)
                .HasColumnName("model_distribution");

            builder.Property(e => e.PercentComponent)
                .HasColumnName("percent_component");


            builder.Property(e => e.ComplementaryID)
                .HasColumnName("complementary_id");

            builder.HasXminRowVersion(e => e.RowVersion);
        }
    }
}