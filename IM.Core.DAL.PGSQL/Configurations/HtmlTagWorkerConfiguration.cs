using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class HtmlTagWorkerConfiguration : HtmlTagWorkerConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_html_tag_worker";

        protected override void ConfigureDatabase(EntityTypeBuilder<HtmlTagWorker> builder)
        {
            builder.ToTable("html_tag_worker", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.QuoteTrimmerID).HasColumnName("quote_trimmer_id");
            builder.Property(x => x.Sequence).HasColumnName("sequence");
            builder.Property(x => x.TagName).HasColumnName("tag_name");
            builder.Property(x => x.Text).HasColumnName("text");
            builder.Property(x => x.TextType).HasColumnName("text_type");
            builder.Property(x => x.Class).HasColumnName("class");
            builder.Property(x => x.ClassType).HasColumnName("class_type");
            builder.Property(x => x.Style).HasColumnName("style");
            builder.Property(x => x.StyleType).HasColumnName("style_type");
            builder.Property(x => x.AttrName).HasColumnName("attr_name");
            builder.Property(x => x.AttrValue).HasColumnName("attr_value");
            builder.Property(x => x.AttrType).HasColumnName("attr_type");
            builder.Property(x => x.LeftID).HasColumnName("left_id");
            builder.Property(x => x.RightID).HasColumnName("right_id");
            builder.Property(x => x.InnerID).HasColumnName("inner_id");
            builder.Property(x => x.Repeat).HasColumnName("repeat");
            builder.Property(x => x.Skip).HasColumnName("skip");
            builder.Property(x => x.InnerNot).HasColumnName("inner_not");
            builder.Property(x => x.RightNot).HasColumnName("right_not");
            builder.Property(x => x.LeftNot).HasColumnName("left_not");
            builder.Property(x => x.OnlyMe).HasColumnName("only_me");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}