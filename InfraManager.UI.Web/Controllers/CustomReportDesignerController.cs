using System.Collections.Generic;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevExpress.DataAccess.Sql;
using InfraManager.DAL.DevExpress;

namespace InfraManager.UI.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomReportDesignerController : ReportDesignerController
    {

        private readonly CustomDevExpressConnectionStringProvider _devExpressConnectionStringProvider;
        
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService,
         CustomDevExpressConnectionStringProvider devExpressConnectionStringProvider) : base(
            controllerService)
        {
            _devExpressConnectionStringProvider = devExpressConnectionStringProvider;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetDesignerModelAsync([FromForm] string reportUrl,
            [FromServices] IReportDesignerClientSideModelGenerator modelGenerator)
        {
            var dataSources = _devExpressConnectionStringProvider.GetReportDataSource();
            
            var model = await modelGenerator.GetModelAsync(reportUrl, dataSources, DefaultUri,
                WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
            return DesignerModel(model);
        }
    }
}
