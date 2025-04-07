using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers;
[ApiExplorerSettings(IgnoreApi = true)]
public class CustomQueryBuilderController : QueryBuilderController
{
    public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
    {
    }
}
