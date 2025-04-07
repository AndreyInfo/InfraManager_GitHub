using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.IM
{
    /// <summary>
    /// Контроллер для работы с инстялляциями
    /// </summary>
    [Route("software-installation")]
    public class SoftwareInstallationController : BaseObjectCrossPlatformController
    {
        protected override int ObjClassID => 71;

        /// <summary>
        /// Инициализирует экземпляр <see cref="LicenceSchemeController"/>.
        /// </summary>        
        public SoftwareInstallationController()
        {
//            _softwareInstallationService = new SoftwareInstallationClient(configuration.WebSettings.CrossPlafromBaseUrlApi, configuration.WebSettings.CrossPlafromApiTokenAuthorization, new UserContextProvider(), new UserDeviceIDProvider());
        }
    }
}