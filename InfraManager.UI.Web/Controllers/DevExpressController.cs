using DevExpress.DashboardCommon;
//using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.ConnectionParameters;
using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace InfraManager.Web.Controllers
{
    public class DevExpressController : BaseController
    {
        // GET: DevExpress
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HtmlEditorPartial()
        {
            return PartialView("_HtmlEditorPartial");
        }

        public ActionResult DashboardIndex()
        {
            var a = Request;
            return PartialView("DashboardViewerPartial");
        }

        //[ValidateInput(false)]
        public ActionResult DashboardViewerPartial()
        {
            return PartialView("DashboardViewerPartial");
        }
    }

    //public class DevExpressControllerHtmlEditorSettings
    //{
    //    public const string ImageUploadDirectory = "~/Content/UploadImages/";
    //    public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

    //    public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
    //    {
    //        AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
    //        MaxFileSize = 4000000
    //    };

    //    static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
    //    public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
    //    {
    //        get
    //        {
    //            if (imageSelectorSettings == null)
    //            {
    //                imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
    //                imageSelectorSettings.Enabled = true;
    //                imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "DevExpress", Action = "HtmlEditorPartialImageSelectorUpload" };
    //                imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
    //                imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
    //                imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
    //                imageSelectorSettings.UploadSettings.Enabled = false;
    //            }
    //            return imageSelectorSettings;
    //        }
    //    }
    //}

}