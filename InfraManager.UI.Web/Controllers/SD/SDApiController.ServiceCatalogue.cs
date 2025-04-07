using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.SD.ServiceCatalogue;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.SD
{
    partial class SDApiController
    {
        #region method GetServiceCategoriesWithServices
        [HttpGet]
        [Route("sdApi/GetServiceCategoriesWithServices", Name = "GetServiceCategoriesWithServices")]
        [Obsolete("Use api/serviceCategories")]
        public DTL.SD.ServiceCatalogue.ServiceCategory[] GetServiceCategoriesWithServices(Guid? userID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDPaiController.GetServiceCategoriesWithServices userID={0}, userName={1}, userID(override)={2}", user.Id, user.UserName, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                var settings = _appSettings.GetConfigurationAsync(false).ConfigureAwait(false).GetAwaiter().GetResult();
                using (var dataSource = DataSource.GetDataSource())
                {
                    var serviceCategoryList = ServiceCategory.GetList(dataSource);
                    var serviceList = Service.GetList(dataSource);
                    var serviceItemAttendanceList = ServiceItemAttendance.GetList(userID.HasValue ? userID.Value : user.User.ID, dataSource);
                    var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(userID.HasValue ? userID.Value : user.User.ID, dataSource);
                    //
                    var retval = serviceCategoryList.
                        Select(x =>
                            new DTL.SD.ServiceCatalogue.ServiceCategory()
                            {
                                ID = x.ID,
                                Name = x.Name,
                                Note = x.Note,
                                ImageSource = x.ImageSource,
                                ServiceList = serviceList.
                                    Where(y => y.ServiceCategoryID == x.ID).
                                    Select(y =>
                                    {
                                        var service = new DTL.SD.ServiceCatalogue.Service();
                                        {
                                            service.ID = y.ID;
                                            service.ServiceCategoryID = y.ServiceCategoryID;
                                            service.Name = y.Name;
                                            service.Note = y.Note;//convert from rtf
                                            service.IsAvailable = availableIDs.Contains(y.ID);
                                            //
                                            bool existsAvailable = false;
                                            service.ServiceItemAttendanceList = serviceItemAttendanceList.
                                                    Where(ia => ia.ServiceID == y.ID).
                                                    Select(ia =>
                                                    {
                                                        var sia = new DTL.SD.ServiceCatalogue.ServiceItemAttendance()
                                                        {
                                                            ID = ia.ID,
                                                            ServiceID = ia.ServiceID,
                                                            ClassID = ia.ClassID,
                                                            Name = ia.Name,
                                                            Note = ia.Note,
                                                            Parameter = ia.Parameter,
                                                            Summary = ia.Summary,
                                                            IsInFavorite = ia.IsInFavorite,
                                                            IsAvailable = service.IsAvailable || availableIDs.Contains(ia.ID)
                                                        };
                                                        if (sia.IsAvailable)
                                                            existsAvailable = true;
                                                        return sia;
                                                    }).
                                                    Where(ia => settings.WebSettings.VisibleNotAvailableServiceBySla || ia.IsAvailable).//запрет показа недоступных настройками
                                                    OrderBy(ia => ia.ClassID).
                                                    ThenBy(ia => ia.Name).
                                                    ToArray();
                                            if (existsAvailable && !service.IsAvailable)
                                                service.IsAvailable = true;
                                        };
                                        return service;
                                    }).
                                    Where(y => settings.WebSettings.VisibleNotAvailableServiceBySla || y.IsAvailable).//запрет показа недоступных настройками
                                    OrderBy(y => y.Name).
                                    ToArray()
                            }).
                        Where(x => x.ServiceList.Length > 0).
                        OrderBy(x => x.Name).
                        ToArray();
                    //
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения категорий и сервисов");
                return null;
            }
        }
        #endregion

        #region method GetServiceItemAttendanceInfo
        [HttpGet]
        [Route("sdApi/GetServiceItemAttendanceInfo", Name = "GetServiceItemAttendanceInfo")]
        [Obsolete("Use api/serviceitems/{id} instead")]
        public DTL.SD.ServiceCatalogue.ServiceItemAttendanceInfo GetServiceItemAttendanceInfo(Guid serviceItemAttendanceID, Guid? userID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDPaiController.GetServiceItemAttendanceInfo userID={0}, userName={1}, serviceItemAttendanceID={2}, userID(override)={3}", user.Id, user.UserName, serviceItemAttendanceID, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var x = ServiceItemAttendanceInfo.Get(serviceItemAttendanceID, dataSource);
                    //TODO alex: не нужно, излишний фильтр препятствует логике. метод используется лишь для уточнения, а не для получения списков (именно в них нужна фильтрация)
                    //var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(userID.HasValue ? userID.Value : user.User.ID, dataSource);
                    ////
                    //if (availableIDs.Contains(x.ServiceID) ||
                    //   (x.ServiceItemID.HasValue && availableIDs.Contains(x.ServiceItemID.Value) ||
                    //   x.ServiceAttendanceID.HasValue && availableIDs.Contains(x.ServiceAttendanceID.Value)))
                    return x.DTL;
                    //
                    //return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информационных данных доступного элемента и услуги клиента");
                return null;
            }
        }
        #endregion

        #region method GetServiceItemAttendanceInfoList
        [HttpGet]
        [Route("sdApi/GetServiceItemAttendanceInfoList", Name = "GetServiceItemAttendanceInfoList")]
        public DTL.SD.ServiceCatalogue.ServiceItemAttendanceInfo[] GetServiceItemAttendanceInfoList(Guid callTypeID, Guid? userID)
        {
            return Array.Empty<DTL.SD.ServiceCatalogue.ServiceItemAttendanceInfo>();
            var user = base.CurrentUser;
            //
            Logger.Trace("SDPaiController.GetServiceItemAttendanceInfoList userID={0}, userName={1}, callTypeID={2}, userID(override)={3}", user.Id, user.UserName, callTypeID, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var serviceItemAttendanceInfoList = ServiceItemAttendanceInfo.GetList(callTypeID, dataSource);//top 50
                    var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(userID.HasValue ? userID.Value : user.User.ID, dataSource);
                    //
                    var siLocalizedText = Resources.ServiceItemCaption;
                    var saLocalizedText = Resources.ServiceAttendanceCaption;
                    var retval = serviceItemAttendanceInfoList.
                        Where(x =>
                            availableIDs.Contains(x.ServiceID) ||
                                (x.ServiceItemID.HasValue && availableIDs.Contains(x.ServiceItemID.Value) ||
                                x.ServiceAttendanceID.HasValue && availableIDs.Contains(x.ServiceAttendanceID.Value))
                             ).
                        Select(x => new DTL.SD.ServiceCatalogue.ServiceItemAttendanceInfo()
                        {
                            FullName = x.FullName,
                            Name = string.Format("{0}: {1}", x.ServiceItemID.HasValue ? siLocalizedText : saLocalizedText, x.Name),
                            ServiceCategoryID = x.ServiceCategoryID,
                            ServiceCategoryName = x.ServiceCategoryName,
                            ServiceID = x.ServiceID,
                            ServiceName = x.ServiceName,
                            ServiceAttendanceID = x.ServiceAttendanceID,
                            ServiceItemID = x.ServiceItemID,
                            Parameter = x.Parameter,
                            Summary = x.Summary
                        }).
                        OrderBy(x => x.FullName).
                        Take(10).
                        ToArray();
                    //
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информационных данных доступных элементов и услуг клиента");
                return null;
            }
        }
        #endregion

        #region method GetServiceItemAttendanceInfoByCallSummaryID
        [HttpGet]
        [Route("sdApi/GetServiceItemAttendanceInfoByCallSummaryID", Name = "GetServiceItemAttendanceInfoByCallSummaryID")]
        public DTL.SD.ServiceCatalogue.ServiceItemAttendanceInfo GetServiceItemAttendanceInfoByCallSummaryID(Guid callSummaryID, Guid? userID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDPaiController.GetServiceItemAttendanceInfoByCallSummaryID userID={0}, userName={1}, callSummaryID={2}, userID(override)={3}", user.Id, user.UserName, callSummaryID, userID.HasValue ? userID.Value.ToString() : "not set");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var serviceItemAttendanceInfo = ServiceItemAttendanceInfo.GetByCallSummaryID(callSummaryID, dataSource);
                    if (serviceItemAttendanceInfo == null)
                        return null;
                    //
                    var availableIDs = ServiceCatalogueHelper.GetAvailableObjectsByUserSLA(userID.HasValue ? userID.Value : user.User.ID, dataSource);
                    //
                    if (availableIDs.Contains(serviceItemAttendanceInfo.ServiceID) ||
                       (serviceItemAttendanceInfo.ServiceItemID.HasValue && availableIDs.Contains(serviceItemAttendanceInfo.ServiceItemID.Value) ||
                       serviceItemAttendanceInfo.ServiceAttendanceID.HasValue && availableIDs.Contains(serviceItemAttendanceInfo.ServiceAttendanceID.Value))
                             )
                        return serviceItemAttendanceInfo.DTL;
                    //
                    return null;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения информационных данных доступных элементов и услуг клиента по краткому описанию");
                return null;
            }
        }
        #endregion
    }
}