using System;
using Inframanager.BLL;
using InfraManager.ResourcesArea;
using Inframanager.BLL.ListView;


namespace InfraManager.BLL.Software.SoftwareModels.ForTable;

[ListViewItem(ListView.SoftwareModelForTable)]
public class SoftwareModelForTable
{
    public Guid ID { get; init; }

    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.Name))]
    public string Name { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.UserNote))]
    public string Note { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.ModelVersion))]
    public string Version { get; set; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.Code))]
    public string Code { get; set; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.SoftwareModel_ManufacturerID))]
    public string ManufacturerID { get; set; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.SoftwareModel_SupportEndDate))]
    public string SupportDate { get; set; }

    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.ModelTemplate))]
    public string TemplateText { get; set; }

    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.SoftwareModel_CreateDate))]
    public DateTime CreateDate { get; set; }

    [ColumnSettings(9, 100)]
    [Label(nameof(Resources.UsingAsset))]
    public string SoftwareModelUsingTypeName { get; set; }

    [ColumnSettings(10, 100)]
    [Label(nameof(Resources.SoftwareCommerdialModel))]
    public bool IsCommercial { get; set; }

    [ColumnSettings(11, 100)]
    [Label(nameof(Resources.SoftwareModel_ProcessNames))]
    public string ProcessNames { get; set; }

    [ColumnSettings(12, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; set; }

    [ColumnSettings(13, 100)]
    [Label(nameof(Resources.ModelRedaction))]
    public string ModelRedaction { get; set; }

    [ColumnSettings(14, 100)]
    [Label(nameof(Resources.SoftwareDistributionModel))]
    public string? ModelDistribution { get; set; }

}
