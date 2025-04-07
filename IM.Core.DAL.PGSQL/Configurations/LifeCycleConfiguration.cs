using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class LifeCycleConfiguration : LifeCycleConfigurationBase
{
    protected override string PrimaryKeyName => "pk_life_cycle_type";
    protected override string LifeCycleStateFK => "fk_life_cycle_states";
    protected override string DefaultValueID => "(gen_random_uuid())";

    protected override string NameUI => "ui_name_life_cycle";

    protected override string FormForeignKey => "fk_life_cycle_form_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycle> builder)
    {
        builder.ToTable("life_cycle", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Removed).HasColumnName("removed");
        builder.Property(e => e.IsFixed).HasColumnName("fixed");
        builder.Property(e => e.Type).HasColumnName("type");
        builder.Property(e => e.FormID).HasColumnName("form_id");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}