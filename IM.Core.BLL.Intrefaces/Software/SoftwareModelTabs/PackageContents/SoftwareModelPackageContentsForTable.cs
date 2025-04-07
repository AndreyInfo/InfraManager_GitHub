using Inframanager.BLL;
using Inframanager.BLL.ListView;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.Software.SoftwareModelTabs.PackageContents;

[ListViewItem(ListView.SoftwareModelPackageContentsForTable)]
public class SoftwareModelPackageContentsForTable
{
    [ColumnSettings(1, 100)]
    [Label(nameof(Resources.SoftwareModelName))]
    public string Name { get; set; }

    [ColumnSettings(2, 100)]
    [Label(nameof(Resources.ModelVersion))]
    public string Version { get; set; }

    [ColumnSettings(3, 100)]
    [Label(nameof(Resources.SoftwareModel_ManufacturerID))]
    public Guid ManufacturerID { get; set; }

    [ColumnSettings(4, 100)]
    [Label(nameof(Resources.SoftwareLicence_ManufacturerName))]
    public string ManufacturerName { get; set; }

    [ColumnSettings(5, 100)]
    [Label(nameof(Resources.SoftwareModel_SoftwareTypeName))]
    public string SoftwareTypeName { get; set; }

    [ColumnSettings(6, 100)]
    [Label(nameof(Resources.ModelTemplate))]
    public string TemplateName { get; set; }

    [ColumnSettings(7, 100)]
    [Label(nameof(Resources.ExternalID))]
    public string ExternalID { get; set; }

    [ColumnSettings(8, 100)]
    [Label(nameof(Resources.Code))]
    public string Code { get; set; }

    [ColumnSettings(9, 100)]
    [Label(nameof(Resources.ModelRedaction))]
    public string? ModelRedaction { get; set; }

    [ColumnSettings(10, 100)]
    [Label(nameof(Resources.ModelLanguage))]
    public string LanguageName { get; set; }

    [ColumnSettings(11, 100)]
    [Label(nameof(Resources.UsingAsset))]
    public string SoftwareModelUsingTypeName { get; set; }

    [ColumnSettings(12, 100)]
    [Label(nameof(Resources.NumberOfInstallations))]
    public int InstallationCount { get; set; }

}
