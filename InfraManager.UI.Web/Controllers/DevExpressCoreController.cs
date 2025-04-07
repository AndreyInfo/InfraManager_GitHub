using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers
{
    public class DevExpressCoreController : DashboardController
    {
        public DevExpressCoreController(DashboardConfigurator configurator, IDataProtectionProvider dataProtectionProvider = null) 
            : base(configurator, dataProtectionProvider)
        {
        }
    }
}
