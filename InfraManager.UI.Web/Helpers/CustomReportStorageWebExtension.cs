using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using InfraManager.BLL.Report;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;

namespace InfraManager.UI.Web.Helpers;

public class CustomReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
{
    private readonly IReportBLL _reportBLL;
    public CustomReportStorageWebExtension(IReportBLL reportBLL)
    {
        _reportBLL = reportBLL;

    }

    public override bool CanSetData(string url) { return true; }
    public override bool IsValidUrl(string url) { return true; }
    public override async Task<byte[]> GetDataAsync(string url)
    {
        var reportData = await _reportBLL.GetReportAsync(Guid.Parse(url), default);
        if (reportData != null)
        {
            return Encoding.Default.GetBytes(reportData.Data);
        }
        throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
        string.Format("Could not find report '{0}'.", url));
    }

    public override async Task<Dictionary<string, string>> GetUrlsAsync()
    {
        var reports = await _reportBLL.GetReportsAsync();
        return reports.ToDictionary(x => x.ID.ToString(), y => y.Name);
    }

    public override async Task SetDataAsync(XtraReport report, string url)
    {
        using var stream = new MemoryStream(); 
        report.SaveLayoutToXml(stream);
        var stringData = Encoding.Default.GetString(stream.ToArray());

        var reportURLDictionary = await GetUrlsAsync(); 
        var reportID = reportURLDictionary.FirstOrDefault(x => x.Key == url).Key;
        await _reportBLL.UpdateReportDataAsync(Guid.Parse(reportID), stringData);
    }

    public override async Task<string> SetNewDataAsync(XtraReport report, string defaultUrl)
    {
        await SetDataAsync(report, defaultUrl);
        return defaultUrl;
    }
}
