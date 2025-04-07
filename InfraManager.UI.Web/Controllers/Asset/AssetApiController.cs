using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.DAL.Web;
using InfraManager.IM.BusinessLayer.Software;
using InfraManager.ResourcesArea;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Assets;
using InfraManager.Web.BLL.Assets.AssetSearch;
using InfraManager.Web.BLL.Contracts;
using InfraManager.Web.BLL.Finance.GoodsInvoice;
using InfraManager.Web.BLL.Helpers;
using InfraManager.Web.BLL.History;
using InfraManager.Web.BLL.Inventories;
using InfraManager.Web.BLL.ProductCatalog.Models;
using InfraManager.Web.BLL.SD;
using InfraManager.Web.BLL.SD.KB;
using InfraManager.Web.DAL;
using InfraManager.Web.DTL;
using InfraManager.Web.DTL.Repository;
using InfraManager.Web.DTL.Settings;
using InfraManager.Web.DTL.Software;
using InfraManager.Web.DTL.Workflow;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SoftwareModel = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelForForm;
using LicenceScheme = InfraManager.Web.BLL.Software.LicenceScheme;
using LicenceSchemeConverter = InfraManager.Web.BLL.Software.LicenceSchemeConverter;
using ListInfo = InfraManager.Web.BLL.ListInfo;
using SoftwareModelViewFilter = InfraManager.Web.BLL.ProductCatalog.Models.SoftwareModelViewFilter;
using SoftwareLicence = InfraManager.Web.BLL.Software.SoftwareLicence;
using SubDeviceParameter = InfraManager.Web.BLL.Assets.SubDeviceParameter;
using System.IO;
using InfraManager.Web.DTL.FormBuilder;
using InfraManager.IM.BusinessLayer.Parameters;
using Newtonsoft.Json;
using static InfraManager.Web.BLL.Parameters.ParameterEnumValue;
using InfraManager.Core.Helpers;
using InfraManager.BLL.Settings;
using InfraManager.BLL;
using InfraManager.Web.BLL.FormBuilder;
using InfraManager.DTL.Web.Synonym;
using InfraManager.BLL.Web.Synonyms;
using InfraManager.BLL.Web.FormBuilder;
using System.Globalization;
using Resources = InfraManager.ResourcesArea.Resources;

namespace InfraManager.Web.Controllers.IM
{
    public sealed partial class AssetApiController : BaseApiController
    {
        private readonly IWebHostEnvironment _environment;

        public AssetApiController(IWebHostEnvironment environment)
        {
            _environment = environment;

        }

        #region method GetNetworkDevice
        public sealed class GetNetworkDeviceOutModel
        {
            public NetworkDevice NetworkDevice { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetNetworkDeviceInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetNetworkDevice", Name = "GetNetworkDevice")]
        public GetNetworkDeviceOutModel GetNetworkDevice(GetNetworkDeviceInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetNetworkDeviceOutModel() { NetworkDevice = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_NETWORKDEVICE))
                {
                    Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetNetworkDeviceOutModel() { NetworkDevice = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = NetworkDevice.Get(model.ID, user.User.ID, dataSource);
                    return new GetNetworkDeviceOutModel() { NetworkDevice = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetNetworkDeviceOutModel() { NetworkDevice = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetNetworkDeviceOutModel() { NetworkDevice = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetAssetModel
        public sealed class GetAssetModelInModel
        {
            public int IntID { get; set; }
            public Guid ID { get; set; }
            public int ClassID { get; set; }
        }

        public sealed class GetAssetModelOutModel
        {
            public AssetModel AssetModel { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetAssetModel", Name = "GetAssetModel")]
        public GetAssetModelOutModel GetAssetModel(GetAssetModelInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetAssetModelOutModel() { AssetModel = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                var operationID = 0;
                switch (model.ClassID)
                {
                    case IMSystem.Global.OBJ_NETWORKDEVICE:
                        operationID = IMSystem.Global.OPERATION_PROPERTIES_NETWORKDEVICE;
                        break;
                    case IMSystem.Global.OBJ_TERMINALDEVICE:
                        operationID = IMSystem.Global.OPERATION_PROPERTIES_TERMINALDEVICE;
                        break;
                    case IMSystem.Global.OBJ_ADAPTER:
                        operationID = IMSystem.Global.OPERATION_PROPERTIES_ADAPTER;
                        break;
                    case IMSystem.Global.OBJ_PERIPHERAL:
                        operationID = IMSystem.Global.OPERATION_PROPERTIES_PERIPHERAL;
                        break;

                }
                if (!user.User.OperationIsGranted(operationID))
                {
                    Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetAssetModelOutModel() { AssetModel = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetModel.Get(model.ID, model.IntID, model.ClassID, user.User.ID, dataSource);
                    return new GetAssetModelOutModel() { AssetModel = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetAssetModelOutModel() { AssetModel = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetAssetModelOutModel() { AssetModel = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetNetworkDeviceModel
        public sealed class GetNetworkDeviceModelOutModel
        {
            public NetworkDeviceModel NetworkDeviceModel { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("imApi/GetNetworkDeviceModel", Name = "GetNetworkDeviceModel")]
        public GetNetworkDeviceModelOutModel GetNetworkDeviceModel(Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetNetworkDeviceModelOutModel() { NetworkDeviceModel = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("imApi/GetNetworkDeviceModel userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_NETWORKDEVICE) &&
                    !user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_NETWORKDEVICE))
                {
                    Logger.Trace("AssetApiController.GetNetworkDeviceModel userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, id);
                    return new GetNetworkDeviceModelOutModel() { NetworkDeviceModel = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = NetworkDeviceModel.Get(id, user.User.ID, dataSource);
                    return new GetNetworkDeviceModelOutModel() { NetworkDeviceModel = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetNetworkDeviceModel is NULL, id: '{0}'", id);
                return new GetNetworkDeviceModelOutModel() { NetworkDeviceModel = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNetworkDeviceModel, id: '{0}'", id);
                return new GetNetworkDeviceModelOutModel() { NetworkDeviceModel = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetSubdeviceParameterList
        public sealed class GetSubdeviceParameterListInModel
        {
            public int DeviceClassID { get; set; }
            public Guid DeviceID { get; set; }
            public int ProductCatalogTemplateID { get; set; }
            public RequestResponceType Result { get; set; }
            public Guid ProductCatalogModelID { get; set; }
        }

        public sealed class GetSubdeviceParameterListOutModel
        {
            public List<SubDeviceParameter> SubDeviceParameterList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("POST")]
        [Route("imApi/GetSubdeviceParameterList", Name = "GetSubdeviceParameterList")]
        public GetSubdeviceParameterListOutModel GetSubdeviceParameterList(GetSubdeviceParameterListInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("imApi/GetSubdeviceParameterList userID={0}, userName={1}, classID={2}, deviceID={3}", user.Id, user.UserName, model.DeviceClassID, model.DeviceID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<SubDeviceParameter> retval;
                    retval = Peripheral.GetSubdeviceParameterList(model.DeviceID, model.DeviceClassID, model.ProductCatalogTemplateID, user.User.ID, dataSource, HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    //

                    return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetNetworkDeviceModel is NULL, id: '{0}'", model.DeviceID);
                return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNetworkDeviceModel, id: '{0}'", model.DeviceID);
                return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpGet]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSubdeviceDefaultParameterList", Name = "GetSubdeviceDefaultParameterList")]
        public GetSubdeviceParameterListOutModel GetSubdeviceDefaultParameterList(GetSubdeviceParameterListInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("assetApi/GetSubdeviceDefaultParameterList userID={0}, userName={1}, classID={2}, deviceID={3}", user.Id, user.UserName, model.DeviceClassID, model.DeviceID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<SubDeviceParameter> retval;
                    retval = Peripheral.GetSubdeviceDefaultParameterList(model.DeviceClassID, model.ProductCatalogTemplateID, model.ProductCatalogModelID, HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                    //

                    return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSubdeviceDefaultParameterList is NULL, id: '{0}'", model.DeviceID);
                return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSubdeviceDefaultParameterList, id: '{0}'", model.DeviceID);
                return new GetSubdeviceParameterListOutModel() { SubDeviceParameterList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetTerminalDevice
        public sealed class GetTerminalDeviceOutModel
        {
            public TerminalDevice TerminalDevice { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetTerminalDeviceInModel
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetTerminalDevice", Name = "GetTerminalDevice")]
        public GetTerminalDeviceOutModel GetTerminalDevice(GetTerminalDeviceInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetTerminalDeviceOutModel() { TerminalDevice = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_TERMINALDEVICE))
                {
                    Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetTerminalDeviceOutModel() { TerminalDevice = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = TerminalDevice.Get(model.ID, user.User.ID, dataSource);
                    return new GetTerminalDeviceOutModel() { TerminalDevice = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetTerminalDeviceOutModel() { TerminalDevice = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetTerminalDeviceOutModel() { TerminalDevice = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetAdapter
        public sealed class GetAdapterOutModel
        {
            public Adapter Adapter { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetAdapterInModel
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetAdapter", Name = "GetAdapter")]
        public GetAdapterOutModel GetAdapter(GetAdapterInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetAdapterOutModel() { Adapter = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Adapter.Get(model.ID, user.User.ID, dataSource);
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_ADAPTER) || !retval.AccessIsGranted)
                    {
                        Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new GetAdapterOutModel() { Adapter = null, Result = RequestResponceType.OperationError };
                    }
                    return new GetAdapterOutModel() { Adapter = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetAdapterOutModel() { Adapter = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetAdapterOutModel() { Adapter = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetPeripheral
        public sealed class GetPeripheralOutModel
        {
            public Peripheral Peripheral { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetPeripheralInModel
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetPeripheral", Name = "GetPeripheral")]
        public GetPeripheralOutModel GetPeripheral(GetPeripheralInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetPeripheralOutModel() { Peripheral = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Peripheral.Get(model.ID, user.User.ID, dataSource);
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_PERIPHERAL) || !retval.AccessIsGranted)
                    {
                        Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new GetPeripheralOutModel() { Peripheral = null, Result = RequestResponceType.OperationError };
                    }
                    return new GetPeripheralOutModel() { Peripheral = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetPeripheralOutModel() { Peripheral = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetPeripheralOutModel() { Peripheral = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetPeripheralModel
        public sealed class GetPeripheralModelOutModel
        {
            public PeripheralModel PeripheralModel { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("imApi/GetPeripheralModel", Name = "GetPeripheralModel")]
        public GetPeripheralModelOutModel GetPeripheralModel(Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetPeripheralModelOutModel() { PeripheralModel = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("imApi/GetNetworkDeviceModel userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_PERIPHERAL) &&
                    !user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_PERIPHERAL))
                {
                    Logger.Trace("AssetApiController.GetPeripheralModel userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, id);
                    return new GetPeripheralModelOutModel() { PeripheralModel = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = PeripheralModel.Get(id, user.User.ID, dataSource);
                    return new GetPeripheralModelOutModel() { PeripheralModel = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetPeripheralModel is NULL, id: '{0}'", id);
                return new GetPeripheralModelOutModel() { PeripheralModel = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPeripheralModel, id: '{0}'", id);
                return new GetPeripheralModelOutModel() { PeripheralModel = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetAssetFields
        public sealed class GetAssetFieldsOutModel
        {
            public AssetFields AssetFields { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetAssetFieldsInModel
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("imApi/GetAssetFields", Name = "GetAssetFields")]
        public GetAssetFieldsOutModel GetAssetFields(GetAssetFieldsInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetAssetFieldsOutModel() { AssetFields = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetFields.Get(model.ID, user.User.ID, dataSource);
                    retval.InitializeUserFieldNames(dataSource);
                    return new GetAssetFieldsOutModel() { AssetFields = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetAssetFieldsOutModel() { AssetFields = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetAssetFieldsOutModel() { AssetFields = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetUserFields
        public sealed class GetUserFieldsOutModel
        {
            public List<DTL.Assets.UserField> UserFields { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetUserFields", Name = "GetUserFields")]
        public GetUserFieldsOutModel GetUserFields()
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetUserFieldsOutModel() { UserFields = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetUserFields userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetFields.InitializeEmptyUserFieldNames(dataSource);
                    return new GetUserFieldsOutModel() { UserFields = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetUserFields is NULL");
                return new GetUserFieldsOutModel() { UserFields = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetUserFields");
                return new GetUserFieldsOutModel() { UserFields = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetPort
        public sealed class GetPortOutModel
        {
            public Port Port { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetPortInModel
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetPort", Name = "GetPort")]
        public GetPortOutModel GetPort(GetPortInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetPortOutModel() { Port = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Port.Get(model.ID, user.User.ID, dataSource);
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_ACTIVEPORT))
                    {
                        Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new GetPortOutModel() { Port = null, Result = RequestResponceType.OperationError };
                    }
                    return new GetPortOutModel() { Port = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetPortOutModel() { Port = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetPortOutModel() { Port = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetDeviceReferenceListList
        public sealed class GetDeviceReferenceListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassID { get; set; }
            public List<int> ReferenceClassList { get; set; }
        }
        public sealed class GetDeviceReferenceListOutModel
        {
            public List<Tuple<int, List<IDeviceReference>, List<InfraManager.Web.DTL.Settings.ColumnSettings>>> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetDeviceReferenceList", Name = "GetDeviceReferenceList")]
        public GetDeviceReferenceListOutModel GetDeviceReferenceList(GetDeviceReferenceListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetDeviceReferenceListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetCallReferenceList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName, model.ID, model.EntityClassID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var columnSettingsList = new List<BLL.Settings.ColumnSettings>();
                    foreach (int classID in model.ReferenceClassList)
                    {
                        string listName = string.Empty;
                        switch (classID)
                        {
                            case IMSystem.Global.OBJ_ADAPTER:
                                listName = "AdapterList";
                                break;
                            case IMSystem.Global.OBJ_PERIPHERAL:
                                listName = "PeripheralList";
                                break;
                            case IMSystem.Global.OBJ_PORT:
                                listName = "PortList";
                                break;
                            case -1://слот
                                listName = "SlotList";
                                break;
                            case IMSystem.Global.OBJ_INSTALLATION:
                                listName = "InstallationList";
                                break;
                        }
                        //
                        var columnSettings = BLL.Settings.ColumnSettings.TryGetOrCreate(user.User, listName, dataSource);
                        columnSettings.ColumnsDTL.Sort((x, y) => x.Order.CompareTo(y.Order));
                        var sortColumn = columnSettings.ColumnsDTL.Where(x => x.SortAsc.HasValue).FirstOrDefault();
                        columnSettingsList.Add(columnSettings);
                    }
                    //
                    var retval = AdapterReference.GetReferenceList(model.ID, model.EntityClassID, model.ReferenceClassList.ToArray(), dataSource);
                    var ret = retval.Select(x => new Tuple<int, List<IDeviceReference>, List<InfraManager.Web.DTL.Settings.ColumnSettings>>(x.Item1, x.Item2, columnSettingsList[retval.IndexOf(x)].ColumnsDTL)).ToList();
                    return new GetDeviceReferenceListOutModel() { List = ret, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetCallReferenceList not supported, model: '{0}'", model));
                return new GetDeviceReferenceListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCallReferenceList, model: {0}.", model);
                return new GetDeviceReferenceListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetAdapterReference
        public sealed class GetAdapterReferenceIncomingModel
        {
            public Guid EntityID { get; set; }
            public int EntityClassID { get; set; }
            public Guid AdapterID { get; set; }
            public bool ReferenceExists { get; set; }
        }
        public sealed class GetAdapterReferenceOutModel
        {
            public AdapterReference Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetAdapterReference", Name = "GetAdapterReference")]
        public GetAdapterReferenceOutModel GetAdapterReference(GetAdapterReferenceIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetAdapterReferenceOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.GetAdapterReference userID={0}, userName={1}, objID={2}, objClassId={3}, AdapterID={4}", user.Id, user.UserName, model.EntityID, model.EntityClassID, model.AdapterID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AdapterReference.Get(model.EntityID, model.EntityClassID, model.AdapterID, dataSource);
                    return new GetAdapterReferenceOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAdapterReference, model: {0}.", model);
                return new GetAdapterReferenceOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetPeripheralReference
        public sealed class GetPeripheralReferenceIncomingModel
        {
            public Guid EntityID { get; set; }
            public int EntityClassID { get; set; }
            public Guid PeripheralID { get; set; }
            public bool ReferenceExists { get; set; }
        }
        public sealed class GetPeripheralReferenceOutModel
        {
            public PeripheralReference Elem { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetPeripheralReference", Name = "GetPeripheralReference")]
        public GetPeripheralReferenceOutModel GetPeripheralReference(GetPeripheralReferenceIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetPeripheralReferenceOutModel() { Elem = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.GetPeripheralReference userID={0}, userName={1}, objID={2}, objClassId={3}, PeripheralID={4}", user.Id, user.UserName, model.EntityID, model.EntityClassID, model.PeripheralID);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = PeripheralReference.Get(model.EntityID, model.EntityClassID, model.PeripheralID, dataSource);
                    return new GetPeripheralReferenceOutModel() { Elem = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetPeripheralReference, model: {0}.", model);
                return new GetPeripheralReferenceOutModel() { Elem = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method RemoveReference
        public sealed class ObjectReferenceModel
        {
            public int ReferenceClassID { get; set; }
            public List<Guid> ReferenceIDList { get; set; }
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveAssetReference", Name = "RemoveAssetReference")]
        public ResultWithMessage RemoveAssetReference(ObjectReferenceModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("SDApiController.RemoveReference userID={0}, userName={1}, ReferenceClassID={2}, ReferenceListCount={3}, ObjectClassID={4}, ObjectID={5}",
            user.Id, user.UserName, model.ReferenceClassID, model.ReferenceIDList.Count, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassID == IMSystem.Global.OBJ_NETWORKDEVICE)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_ADAPTER)
                            NetworkDevice.RemoveAdapterReference(model.ObjectID, model.ReferenceIDList, dataSource);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_PERIPHERAL)
                            NetworkDevice.RemovePeripheralReference(model.ObjectID, model.ReferenceIDList, dataSource);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_INSTALLATIONSOFTWARE)
                            NetworkDevice.RemoveSoftwareInstallationReference(model.ObjectID, model.ReferenceIDList, dataSource);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_TERMINALDEVICE)
                    {
                        if (model.ReferenceClassID == IMSystem.Global.OBJ_ADAPTER)
                            TerminalDevice.RemoveAdapterReference(model.ObjectID, model.ReferenceIDList, dataSource);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_PERIPHERAL)
                            TerminalDevice.RemovePeripheralReference(model.ObjectID, model.ReferenceIDList, dataSource);
                        else if (model.ReferenceClassID == IMSystem.Global.OBJ_INSTALLATIONSOFTWARE)
                            TerminalDevice.RemoveSoftwareInstallationReference(model.ObjectID, model.ReferenceIDList, dataSource);
                    }
                    else
                        throw new NotSupportedException("model.ObjectClassID");
                    //
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении связей.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method GetHistoryList
        public sealed class GetHistoryListIncomingModel
        {
            public Guid ID { get; set; }
            public int EntityClassId { get; set; }
            public string ViewName { get; set; }
            public int StartIdx { get; set; }
            public int? Count { get; set; }
        }
        public sealed class GetHistoryListOutModel
        {
            public IList<HistoryObject> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetAssetHistory", Name = "GetAssetHistory")]
        public GetHistoryListOutModel GetAssetHistory(GetHistoryListIncomingModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetHistoryList userID={0}, userName={1}, objectID={2}, entityClassID={3}", user.Id, user.UserName, model.ID, model.EntityClassId);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    //var asEngineer = (user.User.HasRoles && user.IsEngineerView(model.ViewName));
                    var asEngineer = true;
                    //
                    var retval = HistoryObject.GetList(model.ID, asEngineer, dataSource, model.StartIdx, model.Count);
                    return new GetHistoryListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetHistoryList not supported, model: '{0}'", model));
                return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetHistoryList, model: {0}.", model);
                return new GetHistoryListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetOrgstructureFields
        public sealed class GetOrgstructureFieldsOutModel
        {
            public IList<ListInfo> UserList { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("imApi/GetOrgstructureFields", Name = "GetOrgstructureFields")]
        public GetOrgstructureFieldsOutModel GetOrgstructureFields()
        {
            try
            {                
                var list = BLL.Tables.BaseForTable.GetUserTypesList();
                return new GetOrgstructureFieldsOutModel() { UserList = list, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetOrgstructureFieldsOutModel");
                return new GetOrgstructureFieldsOutModel() { UserList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetDependencyList
        public sealed class GetDependencyListIncomingModel
        {
            public List<Guid> ListID { get; set; }
        }
        public sealed class GetDependencyListOutModel
        {
            public IList<DependencyObject> List { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetAssetLinkList", Name = "GetAssetLinkList")]
        public GetDependencyListOutModel GetAssetLinkList(GetDependencyListIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetDependencyList userID={0}, userName={1}, listIDs={2}", user.Id, user.UserName, model.ListID == null ? "null" : string.Join(",", model.ListID.ToArray()));
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = DependencyObject.GetList(model.ListID, dataSource);
                    return new GetDependencyListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"GetDependencyList not supported, model: '{0}'", model));
                return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetDependencyList, model: {0}.", model);
                return new GetDependencyListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetRegistration
        public sealed class AssetRegistrationOutModel
        {
            public Asset.PrintReportResult? PrintReportResult { get; set; }
            public RequestResponceType Result { get; set; }
            public bool NoDuplicates { get; set; }
            public List<UploadFileInfo> FileInfoList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RegisterAsset", Name = "RegisterAsset")]
        public AssetRegistrationOutModel RegisterAsset(AssetRegistrationInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetRegistrationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.RegisterAsset userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<UploadFileInfo> reportInfoList;
                    Asset.PrintReportResult? printReportResult;
                    var noDuplicates = Asset.RegisterAsset(model, out reportInfoList, out printReportResult);
                    //
                    return new AssetRegistrationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList, NoDuplicates = noDuplicates };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"RegisterAsset not supported, model: '{0}'", model));
                return new AssetRegistrationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RegisterAsset, model: {0}.", model);
                return new AssetRegistrationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region SetFromStorage
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/SetFromStorage", Name = "SetFromStorage")]
        public bool SetFromStorage(AssetFromStorageInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return false;
                //
                Logger.Trace("SDApiController.GetDependencyList userID={0}, userName={1}, objID={2}, objClassId={3}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    Asset.SetFromStorage(model);
                    return true;
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"RegisterAsset not supported, model: '{0}'", model));
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RegisterAsset, model: {0}.", model);
                return false;
            }
        }
        #endregion

        #region AssetToRepair
        public sealed class AssetOperationOutModel
        {
            public Asset.PrintReportResult? PrintReportResult { get; set; }
            public string Error { get; set; }
            public List<UploadFileInfo> FileInfoList { get; set; }
            public RequestResponceType Result { get; set; }
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetToRepair", Name = "AsseToRepair")]
        public AssetOperationOutModel AsseToRepair(AssetToRepairInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetToRepair userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<UploadFileInfo> reportInfoList;
                    Asset.PrintReportResult? printReportResult;
                    //
                    Asset.AssetToRepair(model, out reportInfoList, out printReportResult);
                    //
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"AsseToRepair not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AsseToRepair, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetFromRepair
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetFromRepair", Name = "AssetFromRepair")]
        public AssetOperationOutModel AssetFromRepair(AssetFromRepairInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetFromRepair userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<UploadFileInfo> reportInfoList;
                    Asset.PrintReportResult? printReportResult;
                    //
                    string error = Asset.AssetFromRepair(model, out reportInfoList, out printReportResult);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList, Error = error };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"AssetFromRepair not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AssetFromRepair, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetLicenceConsumption
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetLicenceConsumption", Name = "AssetLicenceConsumption")]
        public AssetOperationOutModel AssetLicenceConsumption(AssetLicenceConsumptionInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.LicenceConsumption userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<UploadFileInfo> reportInfoList;
                    Asset.PrintReportResult? printReportResult;
                    //
                    Asset.AssetLicenceConsumption(model, out reportInfoList, out printReportResult);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"LicenceConsumption not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (ConstraintException ex)
            {
                return new AssetOperationOutModel() { Result = RequestResponceType.ValidationError, Error = ex.Message };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "LicenceConsumption, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetLicenceActive
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetLicenceActive", Name = "AssetLicenceActive")]
        public AssetOperationOutModel AssetLicenceActive(AssetLicenceActiveInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetLicenceActive userID={0}, userName={1}", user.Id, user.UserName);
                //
                List<UploadFileInfo> reportInfoList;
                Asset.PrintReportResult? printReportResult;
                //
                Asset.AssetLicenceActive(model, out reportInfoList, out printReportResult);
                //
                return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"LicenceActive not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "LicenceActive, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetLicenceReturn
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetLicenceReturn", Name = "AssetLicenceReturn")]
        public AssetOperationOutModel AssetLicenceReturn(AssetLicenceReturnInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetLicenceReturnInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<UploadFileInfo> reportInfoList;
                    Asset.PrintReportResult? printReportResult;
                    //
                    Asset.AssetLicenceReturn(model, out reportInfoList, out printReportResult);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"AssetLicenceReturn not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AssetLicenceReturn, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetLicenceUpgrade
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetLicenceUpgradeByContract", Name = "AssetLicenceUpgradeByContract")]
        public AssetOperationOutModel AssetLicenceUpgradeByContract(AssetLicenceUpgradeByContractInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetLicenceUpgrade userID={0}, userName={1}", user.Id, user.UserName);
                //
                List<UploadFileInfo> reportInfoList;
                Asset.PrintReportResult? printReportResult;
                //
                model.User = user.User;
                Asset.AssetLicenceUpgradeByContract(model, out reportInfoList, out printReportResult);
                //
                return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };

            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"LicenceUpgrade not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "LicenceUpgrade, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetLicenceUpgrade", Name = "AssetLicenceUpgrade")]
        public AssetOperationOutModel AssetLicenceUpgrade(AssetLicenceUpgradeInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetLicenceUpgrade userID={0}, userName={1}", user.Id, user.UserName);
                //
                List<UploadFileInfo> reportInfoList;
                Asset.PrintReportResult? printReportResult;
                //
                model.User = user.User;
                Asset.AssetLicenceUpgrade(model, out reportInfoList, out printReportResult);
                //
                return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };

            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"LicenceUpgrade not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "LicenceUpgrade, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetLicenceApply
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetLicenceApply", Name = "AssetLicenceApply")]
        public AssetOperationOutModel AssetLicenceApply(AssetLicenceApplyInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetLicenceApply userID={0}, userName={1}", user.Id, user.UserName);
                //
                List<UploadFileInfo> reportInfoList;
                Asset.PrintReportResult? printReportResult;
                //
                model.User = user.User;
                Asset.AssetLicenceApply(model, out reportInfoList, out printReportResult);
                //
                return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };

            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"LicenceApply not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "LicenceApply, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetWriteOff
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetWriteOff", Name = "AssetWriteOff")]
        public AssetOperationOutModel AssetWriteOff(AssetWriteOffInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetWriteOff userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<UploadFileInfo> reportInfoList;
                    Asset.PrintReportResult? printReportResult;
                    //
                    Asset.AssetWriteOff(model, out reportInfoList, out printReportResult);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"AssetWriteOff not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AssetWriteOff, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AssetMove
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AssetMove", Name = "AssetMove")]
        public AssetOperationOutModel AssetMove(AssetMoveInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.AssetMove userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<UploadFileInfo> reportInfoList;
                    Asset.PrintReportResult? printReportResult;
                    //
                    Asset.AssetMove(model, out reportInfoList, out printReportResult);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success, PrintReportResult = printReportResult, FileInfoList = reportInfoList };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"AssetMove not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError, Error = ex.Message };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AssetMove, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError, Error = ex.Message };
            }
        }
        #endregion

        #region method GetServiceCenterList
        public sealed class GetServiceCenterListOutModel
        {
            public ServiceCenter[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetServiceCenterList", Name = "GetServiceCenterList")]
        public GetServiceCenterListOutModel GetServiceCenterList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetServiceCenterListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetServiceCenterList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("SDApiController.GetServiceCenterList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return new GetServiceCenterListOutModel() { List = null, Result = RequestResponceType.AccessError };
                }
                //
                var retval = ServiceCenter.GetList();
                return new GetServiceCenterListOutModel() { List = retval.ToArray(), Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка причин отклонения от графика.");
                return new GetServiceCenterListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetServiceContractList
        public sealed class GetServiceContractListOutModel
        {
            public ServiceContract[] List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class ServiceCenterInfo
        {
            public Guid ServiceCenterID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetServiceContractList", Name = "GetServiceContractList")]
        public GetServiceContractListOutModel GetServiceContractList(ServiceCenterInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetServiceContractListOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("SDApiController.GetServiceCenterList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("SDApiController.GetServiceCenterList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return new GetServiceContractListOutModel() { List = null, Result = RequestResponceType.AccessError };
                }
                //
                var retval = ServiceContract.GetList(model.ServiceCenterID);
                return new GetServiceContractListOutModel() { List = retval.ToArray(), Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка причин отклонения от графика.");
                return new GetServiceContractListOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetObjectLocation
        public sealed class GetObjectLocationOutModel
        {
            public Guid? LocationID { get; set; }
            public int? LocationIntID { get; set; }
            public int ClassID { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetObjectLocationInModel
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
            public int OperationType { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetObjectLocation", Name = "GetObjectLocation")]
        public GetObjectLocationOutModel GetObjectLocation(GetObjectLocationInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetObjectLocationOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    int? locationIntID;
                    int locationClassID;
                    Guid? locationID;
                    AssetHelper.InitializeLocationControls(model.ClassID, model.ID, model.OperationType, out locationID, out locationClassID, out locationIntID);
                    return new GetObjectLocationOutModel() { LocationID = locationID, LocationIntID = locationIntID, ClassID = locationClassID, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetObjectLocationOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetObjectLocationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSearchedObjectByID
        public sealed class GetModelByIDIncomingModel
        {
            public Guid ModelID { get; set; }
            public int ClassID { get; set; }
        }

        public sealed class GetModelByIDOutModel
        {
            public AssetModelLink Model { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("imApi/GetModelByID", Name = "GetAssetModelByID")]
        public GetModelByIDOutModel GetModelByID(GetModelByIDIncomingModel model)
        {
            var user = base.CurrentUser;

            //
            if (model == null || model.ModelID == Guid.Empty)
                return new GetModelByIDOutModel() { Model = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetModelByID UserID={0}, UserName={1}", user.User.ID, user.User.Name);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = AssetModelLink.Get(model.ModelID, model.ClassID, user.User, dataSource);
                    //
                    return new GetModelByIDOutModel() { Model = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetModelByIDOutModel() { Model = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion



        #region method GetContextMenu
        public sealed class GetContextMenuIncomingModel
        {
            public List<DTL.ObjectInfo> DeviceList { get; set; }
            public Guid? ParentID { get; set; }
            public int? ParentClassID { get; set; }
        }

        public sealed class GetContextMenuIncomingModelWithSubGuid
        {
            public List<DTL.CreateSoftwareLicenceObjectInfo> DeviceList { get; set; }
            public Guid? ParentID { get; set; }
            public int? ParentClassID { get; set; }
        }

        public sealed class GetContextMenuOutModel
        {
            public IList<AssetContextMenu> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("sdApi/GetContextMenu", Name = "GetContextMenu")]
        public GetContextMenuOutModel GetContextMenu(GetContextMenuIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null || model == null)
                {
                    return new GetContextMenuOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                }
                var bllUser = InfraManager.IM.BusinessLayer.User.Get(new Guid(CurrentUser.Id));
                //
                if (model.DeviceList != null)
                {
                    var deviceListStr = string.Format("[{0}]", string.Join(",", model.DeviceList.Select(x => string.Format("<{0},{1}>", x.ID, x.ClassID))));
                    Logger.Trace("AssetApiController.GetContextMenu userID={0}, userName={1}, deviceList={2}", user.Id, user.UserName, deviceListStr);
                    //
                    var menuItemList = AssetContextMenu.Get(
                        bllUser,
                        model.DeviceList,
                        model.ParentClassID,
                        model.ParentID);
                    return new GetContextMenuOutModel() { List = menuItemList, Result = RequestResponceType.Success };
                }
                else if (model.ParentClassID.HasValue && model.ParentID.HasValue)
                {
                    var deviceList = new List<DTL.ObjectInfo>()
                    {
                        new DTL.ObjectInfo()
                        {
                            ClassID = model.ParentClassID.Value,
                            ID = model.ParentID.Value
                        }
                    };
                    var menuItemList = AssetContextMenu.Get(bllUser, deviceList);
                    return new GetContextMenuOutModel() { List = menuItemList, Result = RequestResponceType.Success };
                }
                else
                    return new GetContextMenuOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetContextMenuOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }

        [HttpPost]
        [Route("sdApi/OverrideGetContextMenu", Name = "OverrideGetContextMenu")]
        public GetContextMenuOutModel GetContextMenu(GetContextMenuIncomingModelWithSubGuid model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null || model == null)
                {
                    return new GetContextMenuOutModel() { List = null, Result = RequestResponceType.NullParamsError };
                }

                //
                if (model.DeviceList != null)
                {
                    var deviceListStr = string.Format("[{0}]", string.Join(",", model.DeviceList.Select(x => string.Format("<{0},{1}>", x.ID, x.ClassID))));
                    Logger.Trace("AssetApiController.OverrideGetContextMenu userID={0}, userName={1}, deviceList={2}", user.Id, user.UserName, deviceListStr);
                    //
                    var menuItemList = AssetContextMenu.Get(
                        InfraManager.IM.BusinessLayer.User.Get(new Guid(CurrentUser.Id)),
                        model.DeviceList,
                        model.ParentClassID,
                        model.ParentID);
                    return new GetContextMenuOutModel() { List = menuItemList, Result = RequestResponceType.Success };
                }
                else if (model.ParentClassID.HasValue && model.ParentID.HasValue)
                {
                    var deviceList = new List<DTL.ObjectInfo>()
                    {
                        new DTL.ObjectInfo()
                        {
                            ClassID = model.ParentClassID.Value,
                            ID = model.ParentID.Value
                        }
                    };
                    var menuItemList = AssetContextMenu.Get(InfraManager.IM.BusinessLayer.User.Get(new Guid(CurrentUser.Id)), deviceList);
                    return new GetContextMenuOutModel() { List = menuItemList, Result = RequestResponceType.Success };
                }
                else
                    return new GetContextMenuOutModel() { List = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetContextMenuOutModel() { List = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method ExecuteContextMenu
        public sealed class ExecuteContextMenuIncomingModel
        {
            public List<DTL.ObjectInfo> DeviceList { get; set; }
            public AssetContextMenu Command { get; set; }
        }

        public sealed class CreateSoftwareLicenceFromContextMenuModel
        {
            public List<CreateSoftwareLicenceObjectInfo> DeviceList { get; set; }
            public AssetContextMenu Command { get; set; }
        }

        public sealed class SoftwareBalance
        {
            public string Balance { get; set; }
            public string SoftwareModelName { get; set; }
            public string ManufacturerName { get; set; }
            public string InventoryNumber { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSoftwareLicenceObject", Name = "GetSoftwareLicenceObject")]
        public ResultData<SoftwareBalance> GetSoftwareLicenceObject(Guid ID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultData<SoftwareBalance>.Create(RequestResponceType.NullParamsError);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = new SoftwareBalance();
                    try
                    {
                        var software = SoftwareLicence.Get(ID, user.User.ID, dataSource);
                        retval.Balance = software.Balance;
                        retval.InventoryNumber = software.InventoryNumber;
                        retval.ManufacturerName = software.ManufacturerName;
                        retval.SoftwareModelName = software.SoftwareModelName;
                    }
                    catch
                    {
                        var sub = SoftwareSubLicence.Get(ID, dataSource);
                        retval.InventoryNumber = sub.SoftwareLicence.LV_InvNumber;
                        retval.ManufacturerName = sub.SoftwareLicence.ManufacturerName;
                        retval.SoftwareModelName = sub.SoftwareLicence.SoftwareModelName;

                        var referenceList = sub.ReferenceList
                            .ToArray();

                        var softwareExecutionCounts = referenceList
                            .Where(x => x.SoftwareSubLicenceID == ID)
                            .Select(x => x.SoftwareExecutionCount ?? 0)
                            .ToArray();

                        var count = sub.Count.HasValue
                            ? sub.Count.Value - softwareExecutionCounts.Sum()
                            : 2147483647;

                        retval.Balance = count.ToString();
                    }

                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SOFTWARELICENCE_PROPERTIES))
                    {
                        return ResultData<SoftwareBalance>.Create(RequestResponceType.OperationError);
                    }

                    return ResultData<SoftwareBalance>.Create(RequestResponceType.Success, retval);
                }
            }
            catch (ObjectDeletedException)
            {
                return ResultData<SoftwareBalance>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (Exception)
            {
                return ResultData<SoftwareBalance>.Create(RequestResponceType.GlobalError);
            }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/IsDeviceReference", Name = "IsDeviceReference")]
        public ResultWithMessage IsDeviceReference(List<DTL.ObjectInfo> deviceList)
        {
            var user = base.CurrentUser;
            if (user == null || deviceList == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    foreach (var model in deviceList)
                    {
                        SoftwareLicence retval = null;

                        try
                        {
                            retval = SoftwareLicence.Get(model.ID, user.User.ID, dataSource);
                        }
                        catch
                        {
                            var sub = SoftwareSubLicence.Get(model.ID, dataSource).SoftwareLicence;
                            retval = SoftwareLicence.Get(sub.ID, user.User.ID, dataSource);

                        }

                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SOFTWARELICENCE_PROPERTIES))
                        {
                            return ResultWithMessage.Create(RequestResponceType.OperationError, "OperationError");
                        }

                        var result = retval.SoftwareLicenceSchemeIsDeviceReference;
                        return ResultWithMessage.Create(RequestResponceType.Success, result.ToString(), result);
                    }
                }
            }
            catch (ObjectDeletedException ex)
            {
                return ResultWithMessage.Create(RequestResponceType.ObjectDeleted, ex.Message);
            }
            catch (Exception ex)
            {
                return ResultWithMessage.Create(RequestResponceType.GlobalError, ex.Message);
            }

            return ResultWithMessage.Create(RequestResponceType.Success, bool.FalseString, false);
        }
        //

        [HttpPost]
        [Route("sdApi/ExecuteContextMenu", Name = "ExecuteContextMenu")]
        public ResultWithMessage ExecuteContextMenu(ExecuteContextMenuIncomingModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null || model == null)
                    ResultWithMessage.Create(RequestResponceType.NullParamsError);
                //
                var deviceListStr = string.Format("[{0}]", string.Join(",", model.DeviceList.Select(x => string.Format("<{0},{1}>", x.ClassID, x.ID))));
                var commandStr = string.Format("[command: {0}, lifeCycleStaetOperationID: {1}, name: {2}]", model.Command.CommandType, model.Command.LifeCycleStateOperationID, model.Command.Name);
                Logger.Trace("AssetApiController.GetContextMenu userID={0}, userName={1}, deviceList={2}, command={3}", user.Id, user.UserName, deviceListStr, commandStr);
                //
                var msg = string.Empty;
                var result = AssetContextMenu.Execute(model.DeviceList, model.Command, user.User, out msg);
                return ResultWithMessage.Create(result == true ? RequestResponceType.Success : RequestResponceType.OperationError, msg);
            }
            catch (ArgumentValidationException ex)
            {
                return ResultWithMessage.Create(RequestResponceType.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/CreateSoftwareLicenceFromContextMenu", Name = "CreateSoftwareLicenceFromContextMenu")]
        public ResultWithMessage CreateSoftwareLicenceFromContextMenu(CreateSoftwareLicenceFromContextMenuModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null || model == null)
                    ResultWithMessage.Create(RequestResponceType.NullParamsError);
                //
                var deviceListStr = string.Format("[{0}]", string.Join(",", model.DeviceList.Select(x => string.Format("<{0},{1}>", x.ClassID, x.ID))));
                var commandStr = string.Format("[command: {0}, lifeCycleStaetOperationID: {1}, name: {2}]", model.Command.CommandType, model.Command.LifeCycleStateOperationID, model.Command.Name);
                Logger.Trace("AssetApiController.GetContextMenu userID={0}, userName={1}, deviceList={2}, command={3}", user.Id, user.UserName, deviceListStr, commandStr);
                //
                var msg = string.Empty;
                var result = AssetLicenceCreatorHelper.CreateSoftwareLicence(model.DeviceList, model.Command, out msg);
                return ResultWithMessage.Create(result == true ? RequestResponceType.Success : RequestResponceType.OperationError, msg);
            }
            catch (ArgumentValidationException ex)
            {
                return ResultWithMessage.Create(RequestResponceType.ValidationError, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region CheckInvNumberAndCodeExistence
        public sealed class CheckInvNumberAndCodeExistenceInModel
        {
            public List<DTL.ObjectInfo> DeviceList { get; set; }
        }

        public sealed class CheckInvNumberAndCodeExistenceOutModel
        {
            public bool InvNumberExists { get; set; }
            public bool CodeExists { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/CheckInvNumberAndCodeExistence", Name = "CheckInvNumberAndCodeExistence")]
        public CheckInvNumberAndCodeExistenceOutModel CheckInvNumberAndCodeExistence(CheckInvNumberAndCodeExistenceInModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new CheckInvNumberAndCodeExistenceOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.CheckInvNumberAndCodeExistence userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var tuple = Asset.CheckInvNumberAndCodeExistence(model.DeviceList);
                    //
                    return new CheckInvNumberAndCodeExistenceOutModel() { Result = RequestResponceType.Success, InvNumberExists = tuple.Item1, CodeExists = tuple.Item2 };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CheckInvNumberAndCodeExistence, model: {0}.", model);
                return new CheckInvNumberAndCodeExistenceOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetAssetIdentifierSettingInfo
        public sealed class GetAssetIdentifierSettingInfoOutModel
        {
            public AssetIdentifierSettingInfo Settings { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("imApi/GetAssetIdentifierSettingInfo", Name = "GetAssetIdentifierSettingInfo")]
        public GetAssetIdentifierSettingInfoOutModel GetAssetIdentifierSettingInfo()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetAssetIdentifierSettingInfoOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.GetAssetIdentifierSettingInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                var settings = Asset.GetAssetIdentifierSettingInfo();
                return new GetAssetIdentifierSettingInfoOutModel() { Settings = settings, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetAssetIdentifierSettingInfoOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetAssetLocationInfo
        public sealed class GetAssetLocationInfoOutModel
        {
            public AssetLocationInfo AssetLocationInfo { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/GetAssetLocationInfo", Name = "GetAssetLocationInfo")]
        public GetAssetLocationInfoOutModel GetAssetLocationInfo(GetAssetLocationInfoInModel model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetAssetLocationInfoOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.GetAssetLocationInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                var info = Asset.GetAssetLocationInfo(model);
                return new GetAssetLocationInfoOutModel() { AssetLocationInfo = info, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetAssetLocationInfoOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetCsModelList
        [HttpGet]
        [Route("imApi/GetCsModelList", Name = "GetCsModelList")]
        public DTL.SimpleEnum[] GetCsModelList()
        {
            try
            {
                Logger.Trace("AssetApiController.GetCsModelList");
                var list = NetworkDevice.GetCsModelList();
                //
                var retval = list.
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка CsModel");
                return null;
            }
        }
        #endregion

        #region method GetCPUModelList
        [HttpGet]
        [Route("imApi/GetCPUModelList", Name = "GetCPUModelList")]
        public DTL.SimpleEnumGuid[] GetCPUModelList()
        {
            var user = base.CurrentUser;
            try
            {
                if (user == null)
                    throw new Exception();
                Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName);
                //
                var list = NetworkDevice.GetCPUModelList(user.Id);
                //
                var retval = list.
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка CPUModel");
                return null;
            }
        }
        #endregion
        #region method GetDiskTypeList
        [HttpGet]
        [Route("imApi/GetDiskTypeList", Name = "GetDiskTypeList")]
        public DTL.SimpleEnumGuid[] GetDiskTypeList()
        {
            var user = base.CurrentUser;
            try
            {
                if (user == null)
                    throw new Exception();
                Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName);
                //
                var list = NetworkDevice.GetDiskTypeList(user.Id);
                //
                var retval = list.
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка DiskType");
                return null;
            }
        }
        #endregion

        #region method GetNetworkDeviceCPUInfo
        public sealed class GetDeviceAutoInInfo
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
            public bool CPUAutoInfo { get; set; }
            public bool DiskAutoInfo { get; set; }
            public bool RAMAutoInfo { get; set; }

        }
        public sealed class GetDeviceAutoOutInfo
        {
            public GetDeviceAutoInfo Info { get; set; }
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/GetDeviceAutoInfo", Name = "GetDeviceAutoInfo")]
        public GetDeviceAutoOutInfo GetDeviceAutoInfo(GetDeviceAutoInInfo model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetDeviceAutoOutInfo() { Info = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                //
                var retval = Asset.GetDeviceAutoInfo(model.ID, model.ClassID, model.CPUAutoInfo, model.DiskAutoInfo, model.RAMAutoInfo);
                //
                return new GetDeviceAutoOutInfo() { Info = retval, Result = RequestResponceType.Success };
            }

            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetAsset is NULL, id: '{0}'", model.ID);
                return new GetDeviceAutoOutInfo() { Info = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (NotSupportedException e)
            {
                Logger.Error(e, String.Format(@"GetDeviceAutoInfo bad parameter, model: '{0}'", model));
                return new GetDeviceAutoOutInfo() { Info = null, Result = RequestResponceType.BadParamsError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAsset, id: '{0}'", model.ID);
                return new GetDeviceAutoOutInfo() { Info = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion
        #region method GetCsFormFactorList
        [HttpGet]
        [Route("imApi/GetCsFormFactorList", Name = "GetCsFormFactorList")]
        public DTL.SimpleEnum[] GetCsFormFactorList()
        {
            try
            {
                Logger.Trace("AssetApiController.GetCsFormFactorList");
                var list = NetworkDevice.GetCsFormFactorList();
                //
                var retval = list.
                    OrderBy(x => x.Name).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка CsFormFactor");
                return null;
            }
        }
        #endregion


        #region method GetContract
        public sealed class GetContractOutModel
        {
            public Contract Contract { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetContractInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetContractCost", Name = "GetContractCost")]
        public decimal GetContractCost(Guid Id)
        {
            Logger.Trace("AssetApiController.GetContractCost ID={0}", Id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    return Contract.GetCost(Id, dataSource);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContractCost, id: '{0}'", Id);
                return 0;
            }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetAgreementCost", Name = "GetAgreementCost")]
        public decimal GetAgreementCost(Guid Id)
        {
            Logger.Trace("AssetApiController.GetContractCost ID={0}", Id);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    return ContractAgreement.GetCost(Id, dataSource);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetAgreementCost, id: '{0}'", Id);
                return 0;
            }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetContract", Name = "GetContract")]
        public GetContractOutModel GetContract(GetContractInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetContractOutModel() { Contract = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetContract userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_SERVICECONTRACT))
                {
                    Logger.Trace("AssetApiController.GetContract userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetContractOutModel() { Contract = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Contract.Get(model.ID, user.User.ID, dataSource);
                    return new GetContractOutModel() { Contract = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetContract is NULL, id: '{0}'", model.ID);
                return new GetContractOutModel() { Contract = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContract, id: '{0}'", model.ID);
                return new GetContractOutModel() { Contract = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method RegisterContract
        public sealed class RegisterContractResponse
        {
            public RegisterContractResponse(RequestResponceType type, string message, Guid? contractID)
            {
                this.Type = type;
                this.Message = message;
                this.ContractID = contractID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? ContractID { get; private set; }
        }

        [HttpPost]
        [Route("assetApi/registerContract", Name = "RegisterContract")]
        public RegisterContractResponse RegisterContract(DTL.Contracts.ContractRegistrationInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new RegisterContractResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.RegisterCall userID={0}, userName={1}", user.Id, user.UserName);
            //
            if (!info.ProductCatalogTypeID.HasValue && !info.ServiceContractModelID.HasValue)
                return new RegisterContractResponse(RequestResponceType.BadParamsError, Resources.ErrorCaption, null);
            //
            try
            {
                List<object> documentList;
                List<string> paths;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.Files, out documentList, out paths, user))
                    return new RegisterContractResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    Core.BaseObject bo = Contract.RegisterContract(info, documentList, user.User, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //
                    var obj = Contract.Get(bo.ID, null, dataSource);
                    return new RegisterContractResponse(RequestResponceType.Success, string.Format(Resources.ContractRegistrationMessage, obj.Number), bo.ID);
                }
            }
            catch (DemoVersionException)
            {
                return new RegisterContractResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new RegisterContractResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new RegisterContractResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new RegisterContractResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new RegisterContractResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new RegisterContractResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new RegisterContractResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion


        #region method GetContractAgreement
        public sealed class GetContractAgreementOutModel
        {
            public ContractAgreement ContractAgreement { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetContractAgreementInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetContractAgreement", Name = "GetContractAgreement")]
        public GetContractAgreementOutModel GetContractAgreement(GetContractAgreementInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetContractAgreementOutModel() { ContractAgreement = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetContractAgreement userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ServiceContractAgreement_Properties))
                {
                    Logger.Trace("AssetApiController.GetContractAgreement userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetContractAgreementOutModel() { ContractAgreement = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ContractAgreement.Get(model.ID, dataSource);
                    return new GetContractAgreementOutModel() { ContractAgreement = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetContractAgreement is NULL, id: '{0}'", model.ID);
                return new GetContractAgreementOutModel() { ContractAgreement = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContractAgreement, id: '{0}'", model.ID);
                return new GetContractAgreementOutModel() { ContractAgreement = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method RegisterContractAgreement
        public sealed class RegisterContractAgreementResponse
        {
            public RegisterContractAgreementResponse(RequestResponceType type, string message, Guid? contractAgreementID)
            {
                this.Type = type;
                this.Message = message;
                this.ContractAgreementID = contractAgreementID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? ContractAgreementID { get; private set; }
        }

        [HttpPost]
        [Route("assetApi/registerContractAgreement", Name = "RegisterContractAgreement")]
        public RegisterContractAgreementResponse RegisterContractAgreement(DTL.Contracts.ContractAgreementRegistration info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new RegisterContractAgreementResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("SDApiController.RegisterAgreement userID={0}, userName={1}", user.Id, user.UserName);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    Core.BaseObject bo = ContractAgreement.RegisterAgreement(info, dataSource);
                    dataSource.CommitTransaction();
                    //
                    var obj = ContractAgreement.Get(bo.ID, dataSource);
                    return new RegisterContractAgreementResponse(RequestResponceType.Success, string.Format(Resources.ContractAgreementRegistrationMessage, obj.Number), bo.ID);
                }
            }
            catch (DemoVersionException)
            {
                return new RegisterContractAgreementResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new RegisterContractAgreementResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new RegisterContractAgreementResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new RegisterContractAgreementResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new RegisterContractAgreementResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new RegisterContractAgreementResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new RegisterContractAgreementResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion


        #region GetContractModel
        public sealed class GetContractModelOutModel
        {
            public ContractModel Model { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("imApi/GetContractModel", Name = "GetContractModel")]
        public GetContractModelOutModel GetContractModel(Guid id)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetContractModelOutModel() { Model = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("imApi/GetContractModel userID={0}, userName={1}, ID={2}", user.Id, user.UserName, id);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_SERVICECONTRACT) &&
                    !user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_SERVICECONTRACT))
                {
                    Logger.Trace("AssetApiController.GetContractModel userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, id);
                    return new GetContractModelOutModel() { Model = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ContractModel.Get(id, dataSource);
                    return new GetContractModelOutModel() { Model = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetContractModel is NULL, id: '{0}'", id);
                return new GetContractModelOutModel() { Model = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContractModel, id: '{0}'", id);
                return new GetContractModelOutModel() { Model = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSupplier
        public sealed class GetSupplierOutModel
        {
            public Supplier Supplier { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetSupplierInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSupplier", Name = "GetSupplier")]
        public GetSupplierOutModel GetSupplier(GetSupplierInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSupplierOutModel() { Supplier = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetSupplier userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_NETWORKDEVICE))
                {
                    Logger.Trace("AssetApiController.GetSupplier userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetSupplierOutModel() { Supplier = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Supplier.Get(model.ID, user.User.ID, dataSource);
                    return new GetSupplierOutModel() { Supplier = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSupplier is NULL, id: '{0}'", model.ID);
                return new GetSupplierOutModel() { Supplier = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSupplier, id: '{0}'", model.ID);
                return new GetSupplierOutModel() { Supplier = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSupplierTypeList
        public sealed class GetSupplierTypeListOutModel
        {
            public List<DTL.Assets.SupplierType> TypeList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSupplierTypeList", Name = "GetSupplierTypeList")]
        public GetSupplierTypeListOutModel GetSupplierTypeList()
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSupplierTypeListOutModel() { TypeList = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetSupplierTypeList userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_NETWORKDEVICE))
                {
                    Logger.Trace("AssetApiController.GetSupplierTypeList userID={0}, userName={1}", user.Id, user.UserName);
                    return new GetSupplierTypeListOutModel() { TypeList = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Supplier.GetTypeList(dataSource);
                    return new GetSupplierTypeListOutModel() { TypeList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSupplierTypeList");
                return new GetSupplierTypeListOutModel() { TypeList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSupplierContactPerson
        public sealed class GetSupplierContactPersonOutModel
        {
            public ContactPerson SupplierContactPerson { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetSupplierContactPersonInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSupplierContactPerson", Name = "GetSupplierContactPerson")]
        public GetSupplierContactPersonOutModel GetSupplierContactPerson(GetSupplierContactPersonInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSupplierContactPersonOutModel() { SupplierContactPerson = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetSupplierContactPerson userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_NETWORKDEVICE))
                {
                    Logger.Trace("AssetApiController.GetSupplierContactPerson userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetSupplierContactPersonOutModel() { SupplierContactPerson = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ContactPerson.Get(model.ID, user.User.ID, dataSource);
                    return new GetSupplierContactPersonOutModel() { SupplierContactPerson = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSupplierContactPerson is NULL, id: '{0}'", model.ID);
                return new GetSupplierContactPersonOutModel() { SupplierContactPerson = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSupplierContactPerson, id: '{0}'", model.ID);
                return new GetSupplierContactPersonOutModel() { SupplierContactPerson = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method EditSupplierContactPerson
        public sealed class EditSupplierContactPersonOutModel
        {
            public ContactPerson NewModel { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("assetApi/EditSupplierContactPerson", Name = "EditSupplierContactPerson")]
        public EditSupplierContactPersonOutModel EditSupplierContactPerson(DTL.Assets.SupplierContactPerson model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    int operationID;
                    if (!model.ID.HasValue)
                        operationID = IMSystem.Global.OPERATION_SupplierContactPerson_Add;
                    else
                        operationID = IMSystem.Global.OPERATION_SupplierContactPerson_Update;
                    //
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("FinanceApiController.EditSupplierContactPerson userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditSupplierContactPersonOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        var result = ContactPerson.Edit(model, user.User.ID, dataSource);
                        //
                        return new EditSupplierContactPersonOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewModel = result };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditSupplierContactPerson not supported, model: '{0}'", model));
                    return new EditSupplierContactPersonOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditSupplierContactPerson object deleted, model: '{0}'", model));
                    return new EditSupplierContactPersonOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditSupplierContactPerson validation error, model: '{0}'", model));
                    return new EditSupplierContactPersonOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditSupplierContactPerson, model: '{0}'", model));
                    return new EditSupplierContactPersonOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditSupplierContactPersonOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method EditSupplier
        public sealed class EditSupplierOutModel
        {
            public Supplier NewModel { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("assetApi/EditSupplier", Name = "EditSupplier")]
        public EditSupplierOutModel EditSupplier(DTL.Assets.Supplier model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    int operationID;
                    if (!model.ID.HasValue)
                        operationID = IMSystem.Global.OPERATION_ADD_SUPPLIER;
                    else
                        operationID = IMSystem.Global.OPERATION_UPDATE_SUPPLIER;
                    //
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("FinanceApiController.EditSupplier userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditSupplierOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        var result = Supplier.Edit(model, user.User.ID, dataSource);
                        //
                        return new EditSupplierOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewModel = result };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditSupplier not supported, model: '{0}'", model));
                    return new EditSupplierOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditSupplier object deleted, model: '{0}'", model));
                    return new EditSupplierOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditSupplier validation error, model: '{0}'", model));
                    return new EditSupplierOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditSupplier, model: '{0}'", model));
                    return new EditSupplierOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditSupplierOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method EditContractMaintenance
        public sealed class EditContractMaintenanceOutModel
        {
            public ContractMaintenance NewModel { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("assetApi/EditContractMaintenance", Name = "EditContractMaintenance")]
        public EditContractMaintenanceOutModel EditContractMaintenance(DTL.Contracts.Maintenance model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ServiceContractMaintenance_Update))
                    {
                        Logger.Trace("FinanceApiController.EditContractMaintenance userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditContractMaintenanceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        var result = ContractMaintenance.Edit(model, user.User.ID, dataSource);
                        //
                        return new EditContractMaintenanceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewModel = result };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditContractMaintenance not supported, model: '{0}'", model));
                    return new EditContractMaintenanceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditContractMaintenance object deleted, model: '{0}'", model));
                    return new EditContractMaintenanceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditContractMaintenance validation error, model: '{0}'", model));
                    return new EditContractMaintenanceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditContractMaintenance, model: '{0}'", model));
                    return new EditContractMaintenanceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditContractMaintenanceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method GetContractMaintenance
        public sealed class GetContractMaintenanceOutModel
        {
            public ContractMaintenance ContractMaintenance { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetContractMaintenanceInModel
        {
            public Guid ServiceContractID { get; set; }
            public Guid ObjectID { get; set; }
            public int ObjectClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetContractMaintenance", Name = "GetContractMaintenance")]
        public GetContractMaintenanceOutModel GetContractMaintenance(GetContractMaintenanceInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetContractMaintenanceOutModel() { ContractMaintenance = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetContractMaintenance userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ObjectID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ServiceContractMaintenance_Properties))
                {
                    Logger.Trace("AssetApiController.GetContractMaintenance userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ObjectID);
                    return new GetContractMaintenanceOutModel() { ContractMaintenance = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ContractMaintenance.Get(model.ServiceContractID, model.ObjectID, model.ObjectClassID, user.User.ID, dataSource);
                    return new GetContractMaintenanceOutModel() { ContractMaintenance = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetContractMaintenance is NULL, id: '{0}'", model.ObjectID);
                return new GetContractMaintenanceOutModel() { ContractMaintenance = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContractMaintenance, id: '{0}'", model.ObjectID);
                return new GetContractMaintenanceOutModel() { ContractMaintenance = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method AddContractMaintenance
        public sealed class AddMaintenanceInputModel
        {
            public List<DTL.ObjectInfo> DependencyList { get; set; }
            public Guid ContractID { get; set; }
        }
        public sealed class AddMaintenanceOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddContractMaintenance", Name = "AddContractMaintenance")]
        public AddMaintenanceOutModel AddContractMaintenance(AddMaintenanceInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.AddContractMaintenance userID={0}, userName={1}, ReferenceListCount={2}, ContractID={3}",
            user.Id, user.UserName, model.DependencyList.Count, model.ContractID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    ContractMaintenance.AddMaintenance(model.ContractID, model.DependencyList, dataSource);
                    //
                    dataSource.CommitTransaction();
                    return new AddMaintenanceOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddContractMaintenance concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ContractID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddContractMaintenance OBJECT concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ContractID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"AddContractMaintenance object deleted error. ID: '{0}'. Assets: '{1}'",
                        model.ContractID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method AddAgreementMaintenance
        public sealed class AddAgreementMaintenanceInputModel
        {
            public List<DTL.ObjectInfo> DependencyList { get; set; }
            public Guid AgreementID { get; set; }
        }
        public sealed class AddAgreementMaintenanceOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddAgreementMaintenance", Name = "AddAgreementMaintenance")]
        public AddMaintenanceOutModel AddAgreementMaintenance(AddAgreementMaintenanceInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.AddContractMaintenance userID={0}, userName={1}, ReferenceListCount={2}, ContractID={3}",
            user.Id, user.UserName, model.DependencyList.Count, model.AgreementID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    ContractMaintenance.AddAgreementMaintenance(model.AgreementID, model.DependencyList, dataSource);
                    //
                    dataSource.CommitTransaction();
                    return new AddMaintenanceOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddAgreementMaintenance concurency error. ID: '{0}'. Assets: '{1}'",
                        model.AgreementID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddAgreementMaintenance OBJECT concurency error. ID: '{0}'. Assets: '{1}'",
                        model.AgreementID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"AddAgreementMaintenance object deleted error. ID: '{0}'. Assets: '{1}'",
                        model.AgreementID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddMaintenanceOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method GetContractUserList
        public sealed class GetContractUserListOutModel
        {
            public List<DTL.Users.UserInfo> ContactPersonList { get; set; }
            public List<DTL.Users.UserInfo> UserList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetContractUserList", Name = "GetContractUserList")]
        public GetContractUserListOutModel GetContractUserList(Guid contractID)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetContractUserListOutModel() { ContactPersonList = null, UserList = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetContractUserList userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_SERVICECONTRACT))
                {
                    Logger.Trace("AssetApiController.GetContractUserList userID={0}, userName={1}", user.Id, user.UserName);
                    return new GetContractUserListOutModel() { ContactPersonList = null, UserList = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Contract.GetUserList(contractID, dataSource);
                    return new GetContractUserListOutModel() { ContactPersonList = retval.Item1, UserList = retval.Item2, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContractUserList");
                return new GetContractUserListOutModel() { ContactPersonList = null, UserList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method ContractRemoveUser
        public sealed class ContractRemoveUserInModel
        {
            public Guid ContractID { get; set; }
            public Guid ObjectID { get; set; }
            public int ObjectClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("POST")]
        [Route("assetApi/ContractRemoveUser", Name = "ContractRemoveUser")]
        public RequestResponceType ContractRemoveUser(ContractRemoveUserInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.NullParamsError;
            //
            Logger.Trace("AssetApiController.ContractRemoveUser userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_SERVICECONTRACT))
                {
                    Logger.Trace("AssetApiController.GetContractUserList userID={0}, userName={1}", user.Id, user.UserName);
                    return RequestResponceType.OperationError;
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    Contract.RemoveUser(model.ContractID, model.ObjectClassID, model.ObjectID, dataSource);
                    return RequestResponceType.Success;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ContractRemoveUser");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region method AddContactPerson
        public sealed class AddContactPersonInputModel
        {
            public List<DTL.ObjectInfo> DependencyList { get; set; }
            public Guid ContractID { get; set; }
        }
        public sealed class AddContactPersonOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddContactPerson", Name = "AddContactPerson")]
        public AddContactPersonOutModel AddContactPerson(AddContactPersonInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddContactPersonOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.AddContactPerson userID={0}, userName={1}, ReferenceListCount={2}, ContractID={4}",
            user.Id, user.UserName, model.DependencyList.Count, model.ContractID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    Contract.AddContactPersonList(model.ContractID, model.DependencyList, dataSource);
                    //
                    dataSource.CommitTransaction();
                    return new AddContactPersonOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddContactPerson concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ContractID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddContactPersonOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddContactPerson OBJECT concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ContractID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddContactPersonOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"AddContactPerson object deleted error. ID: '{0}'. Assets: '{1}'",
                        model.ContractID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddContactPersonOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddContactPersonOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method RegisterAdapter
        public sealed class RegisterAdapterResponse
        {
            public RegisterAdapterResponse(RequestResponceType type, string message, Guid? contractID)
            {
                this.Type = type;
                this.Message = message;
                this.AdapterID = contractID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? AdapterID { get; private set; }
        }

        public sealed class AdapterInfo
        {
            public DTL.Assets.Adapter Adapter { get; set; }
            public DTL.Assets.AssetFields AssetFields { get; set; }
        }


        [HttpPost]
        [Route("assetApi/registerAdapter", Name = "RegisterAdapter")]
        public RegisterAdapterResponse RegisterAdapter(AdapterInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new RegisterAdapterResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("AssetApiController.RegisterAdapter userID={0}, userName={1}", user.Id, user.UserName);
            //
            /*if (!info.ProductCatalogTypeID.HasValue && !info.ServiceAdapterModelID.HasValue)
                return new RegisterAdapterResponse(RequestResponceType.BadParamsError, Resources.ErrorCaption, null);*/
            //
            try
            {
                List<object> documentList;
                List<string> paths;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.Adapter.Files, out documentList, out paths, user))
                    return new RegisterAdapterResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    Adapter.RegisterAdapter(info.Adapter, info.AssetFields, documentList, user.User, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //
                    //var obj = Adapter.Get(bo.ID, null, dataSource);
                    return new RegisterAdapterResponse(RequestResponceType.Success, string.Format(Resources.AssetRegistration_Success), null);
                }
            }
            catch (DemoVersionException)
            {
                return new RegisterAdapterResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new RegisterAdapterResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new RegisterAdapterResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new RegisterAdapterResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new RegisterAdapterResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new RegisterAdapterResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new RegisterAdapterResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion

        #region method RegisterPeripheral
        public sealed class RegisterPeripheralResponse
        {
            public RegisterPeripheralResponse(RequestResponceType type, string message, Guid? contractID)
            {
                this.Type = type;
                this.Message = message;
                this.PeripheralID = contractID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? PeripheralID { get; private set; }
        }

        public sealed class PeripheralInfo
        {
            public DTL.Assets.Peripheral Peripheral { get; set; }
            public DTL.Assets.AssetFields AssetFields { get; set; }
        }


        [HttpPost]
        [Route("assetApi/registerPeripheral", Name = "RegisterPeripheral")]
        public RegisterPeripheralResponse RegisterPeripheral(PeripheralInfo info)
        {
            var user = base.CurrentUser;
            if (info == null || user == null)
                return new RegisterPeripheralResponse(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("AssetApiController.RegisterPeripheral userID={0}, userName={1}", user.Id, user.UserName);
            //
            /*if (!info.ProductCatalogTypeID.HasValue && !info.ServicePeripheralModelID.HasValue)
                return new RegisterPeripheralResponse(RequestResponceType.BadParamsError, Resources.ErrorCaption, null);*/
            //
            try
            {
                List<object> documentList;
                List<string> paths;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(info.Peripheral.Files, out documentList, out paths, user))
                    return new RegisterPeripheralResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                    Peripheral.RegisterPeripheral(info.Peripheral, info.AssetFields, documentList, user.User, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //
                    //var obj = Peripheral.Get(bo.ID, null, dataSource);
                    return new RegisterPeripheralResponse(RequestResponceType.Success, string.Format(Resources.AssetRegistration_Success), null);
                }
            }
            catch (DemoVersionException)
            {
                return new RegisterPeripheralResponse(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new RegisterPeripheralResponse(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new RegisterPeripheralResponse(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new RegisterPeripheralResponse(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new RegisterPeripheralResponse(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new RegisterPeripheralResponse(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new RegisterPeripheralResponse(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }
        #endregion

        #region method GetContractLicence
        public sealed class GetContractLicenceOutModel
        {
            public ContractLicence ContractLicence { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetContractLicenceInModel
        {
            public Guid ID { get; set; }
            public bool IsAgreementLicence { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetContractLicence", Name = "GetContractLicence")]
        public GetContractLicenceOutModel GetContractLicence(GetContractLicenceInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetContractLicenceOutModel() { ContractLicence = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetContractLicence userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ServiceContractLicence_Properties))
                {
                    Logger.Trace("AssetApiController.GetContractLicence userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetContractLicenceOutModel() { ContractLicence = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = new ContractLicence(null);
                    if (model.IsAgreementLicence)
                    {
                        retval = ContractLicence.GetForAgreement(model.ID, user.User.ID, dataSource);
                    }
                    else
                    {
                        retval = ContractLicence.Get(model.ID, user.User.ID, dataSource);
                    }
                    return new GetContractLicenceOutModel() { ContractLicence = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetContractLicence is NULL, id: '{0}'", model.ID);
                return new GetContractLicenceOutModel() { ContractLicence = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetContractLicence, id: '{0}'", model.ID);
                return new GetContractLicenceOutModel() { ContractLicence = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetProductCatalogClass
        public sealed class GetProductCatalogOutClass
        {
            public int? ProductCatalogClass { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetProductCatalogInClass
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetProductCatalogClass", Name = "GetProductCatalogClass")]
        public GetProductCatalogOutClass GetProductCatalogClass(GetProductCatalogInClass model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetProductCatalogOutClass() { ProductCatalogClass = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetProductCatalogClass userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {

                    int? retval = ContractLicence.GetLicenceClass(model.ID, user.User.ID, dataSource);
                    return new GetProductCatalogOutClass() { ProductCatalogClass = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProductCatalogClass, id: '{0}'", model.ID);
                return new GetProductCatalogOutClass() { ProductCatalogClass = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method EditContractLicence
        public sealed class EditContractLicenceOutModel
        {
            public ContractLicence NewModel { get; set; }
            public ResultWithMessage Response { get; set; }
        }
        [HttpPost]
        [Route("assetApi/EditContractLicence", Name = "EditContractLicence")]
        public EditContractLicenceOutModel EditContractLicence(DTL.Contracts.ContractLicence model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    int operationID;
                    if (!model.ID.HasValue)
                        operationID = IMSystem.Global.OPERATION_ServiceContractLicence_Add;
                    else
                        operationID = IMSystem.Global.OPERATION_ServiceContractLicence_Update;
                    //
                    if (!user.User.OperationIsGranted(operationID))
                    {
                        Logger.Trace("FinanceApiController.EditContractLicence userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new EditContractLicenceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.OperationError) };
                    }
                    //
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        ContractLicence result;
                        if (model.IsContractAgreement)
                        {
                            result = ContractLicence.EditAgreementLicence(model, user.User.ID, dataSource);
                        }
                        else
                        {
                            result = ContractLicence.Edit(model, user.User.ID, dataSource);
                        }
                        //
                        return new EditContractLicenceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.Success), NewModel = result };
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditContractLicence not supported, model: '{0}'", model));
                    return new EditContractLicenceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.BadParamsError) };
                }
                catch (ObjectDeletedException)
                {
                    Logger.Trace(String.Format(@"EditContractLicence object deleted, model: '{0}'", model));
                    return new EditContractLicenceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ObjectDeleted) };
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Trace(String.Format(@"EditContractLicence validation error, model: '{0}'", model));
                    return new EditContractLicenceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.ValidationError, ValidatorHelper.CreateErrorMessage(e)) };
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditContractLicence, model: '{0}'", model));
                    return new EditContractLicenceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.GlobalError) };
                }
            }
            else return new EditContractLicenceOutModel() { Response = ResultWithMessage.Create(RequestResponceType.NullParamsError) };
        }
        #endregion

        #region method GetSoftwareLicence
        public sealed class GetSoftwareLicenceOutModel
        {
            public SoftwareLicence SoftwareLicence { get; set; }
            public RequestResponceType Result { get; set; }
            public DeviceInfo OEMDevice { get; set; }
            public bool IsSiteLicenceScheme { get; set; }
        }

        public sealed class DeviceInfo
        {
            public string FullName { get; set; }
            public string Location { get; set; }
            public string TypeName { get; set; }
        }

        public sealed class GetSoftwareLicenceInModel
        {
            public Guid ID { get; set; }
            public int ClassID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetSoftwareLicence", Name = "GetSoftwareLicence")]
        public GetSoftwareLicenceOutModel GetSoftwareLicence(GetSoftwareLicenceInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSoftwareLicenceOutModel() { SoftwareLicence = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetSoftwareLicence userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = SoftwareLicence.Get(model.ID, user.User.ID, dataSource);
                    var lScheme = SoftwareLicenceScheme.Get(retval.SoftwareLicenceSchemeID, dataSource);
                    DeviceInfo deviceModel = null;
                    //List<SoftwareLicenceLocationForTable> locations = null;
                    if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_SOFTWARELICENCE_PROPERTIES))
                    {
                        Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                        return new GetSoftwareLicenceOutModel() { SoftwareLicence = null, Result = RequestResponceType.OperationError };
                    }
                    if (retval.OEMDeviceID != null && retval.OEMDeviceClassID != null)
                    {
                        string fullName, location, type;
                        InfraManager.IM.BusinessLayer.Asset.AssetService.GetObjectInfo(retval.OEMDeviceClassID.Value, retval.OEMDeviceID.Value, out fullName, out location, dataSource);
                        type = InfraManager.IM.BusinessLayer.ClassIM.ClassIM.GetName(retval.OEMDeviceClassID.Value);

                        deviceModel = new DeviceInfo()
                        {
                            FullName = fullName,
                            Location = location,
                            TypeName = type,
                        };
                    }

                    return new GetSoftwareLicenceOutModel()
                    {
                        SoftwareLicence = retval,
                        Result = RequestResponceType.Success,
                        OEMDevice = deviceModel,
                        IsSiteLicenceScheme = (lScheme?.CompatibilitySoftwareLicenceScheme == InfraManager.IM.BusinessLayer.Software.LicenceScheme.Site),
                    };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSoftwareLicence is NULL, id: '{0}'", model.ID);
                return new GetSoftwareLicenceOutModel() { SoftwareLicence = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSoftwareLicence, id: '{0}'", model.ID);
                return new GetSoftwareLicenceOutModel() { SoftwareLicence = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method AddSoftwareLicence

        public sealed class AddSoftwareLicenceOutModel
        {
            public AddSoftwareLicenceOutModel(RequestResponceType type, string message, Guid? softwareLicenceID)
            {
                this.Type = type;
                this.Message = message;
                this.SoftwareLicenceID = softwareLicenceID;
            }

            public RequestResponceType Type { get; private set; }
            public string Message { get; private set; }
            public Guid? SoftwareLicenceID { get; private set; }
        }

        public sealed class AddSoftwareLicenceInModel
        {
            public DTL.Software.SoftwareLicence Licence { get; set; }
            public List<DTL.Software.SoftwareLicenceLocation> LocationRestrictions { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddSoftwareLicence", Name = "AddSoftwareLicence")]
        public AddSoftwareLicenceOutModel AddSoftwareLicence(AddSoftwareLicenceInModel addSoftwareLicenceInModel)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddSoftwareLicenceOutModel(RequestResponceType.NullParamsError, Resources.ErrorCaption, null);
            //
            Logger.Trace("AssetApiController.AddSoftwareLicence userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                List<object> documentList;
                List<string> paths;
                var api = new FileApiController(_environment);
                if (!api.GetDocumentFromFiles(addSoftwareLicenceInModel.Licence.Files, out documentList, out paths, user))
                    return new AddSoftwareLicenceOutModel(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);

                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction(IsolationLevel.RepeatableRead);
                    var sofwareLicenceID = SoftwareLicence.Insert(addSoftwareLicenceInModel.Licence, documentList, addSoftwareLicenceInModel.LocationRestrictions, dataSource);
                    dataSource.CommitTransaction();
                    //                    
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);

                    return new AddSoftwareLicenceOutModel(RequestResponceType.Success, string.Format(Resources.AssetRegistration_Success), sofwareLicenceID);
                }
            }
            catch (DemoVersionException)
            {
                return new AddSoftwareLicenceOutModel(RequestResponceType.AccessError, Resources.DemoVersionException, null);
            }
            catch (OutOfMemoryException ex)
            {
                Logger.Warning(ex);
                return new AddSoftwareLicenceOutModel(RequestResponceType.GlobalError, Resources.OutOfMemoryException, null);
            }
            catch (ArgumentValidationException ex)
            {
                return new AddSoftwareLicenceOutModel(RequestResponceType.BadParamsError, string.Format(Resources.ArgumentValidationException, ex.Message), null);
            }
            catch (ObjectInUseException)
            {
                return new AddSoftwareLicenceOutModel(RequestResponceType.ConcurrencyError, Resources.ConcurrencyError, null);
            }
            catch (ObjectConstraintException)
            {
                return new AddSoftwareLicenceOutModel(RequestResponceType.BadParamsError, Resources.SaveError, null);
            }
            catch (ObjectDeletedException)
            {
                return new AddSoftwareLicenceOutModel(RequestResponceType.ObjectDeleted, Resources.SaveError, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new AddSoftwareLicenceOutModel(RequestResponceType.GlobalError, Resources.ErrorCaption, null);
            }
        }

        #endregion

        #region method GetSoftwareOEMLicenceDevice
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/GetSoftwareOEMLicenceDevice", Name = "GetSoftwareOEMLicenceDevice")]
        public GetSoftwareLicenceOutModel GetSoftwareOEMLicenceDevice(GetSoftwareLicenceInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSoftwareLicenceOutModel() { SoftwareLicence = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetSoftwareLicence userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    DeviceInfo deviceModel = null;
                    string fullName, location, type;
                    InfraManager.IM.BusinessLayer.Asset.AssetService.GetObjectInfo(model.ClassID, model.ID, out fullName, out location, dataSource);
                    type = InfraManager.IM.BusinessLayer.ClassIM.ClassIM.GetName(model.ClassID);

                    deviceModel = new DeviceInfo()
                    {
                        FullName = fullName,
                        Location = location,
                        TypeName = type,
                    };
                    return new GetSoftwareLicenceOutModel() { Result = RequestResponceType.Success, OEMDevice = deviceModel, SoftwareLicence = null };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSoftwareLicence is NULL, id: '{0}'", model.ID);
                return new GetSoftwareLicenceOutModel() { SoftwareLicence = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSoftwareLicence, id: '{0}'", model.ID);
                return new GetSoftwareLicenceOutModel() { SoftwareLicence = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method CanChangeParent
        public sealed class CanChangeSoftwareLicenceParentOutModel
        {
            public bool? CanChange { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class CanSoftwareLicenceChangeParentInModel
        {
            public Guid ID { get; set; }
            public Guid ParentID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/CanChangeSoftwareLicenceParent", Name = "CanChangeSoftwareLicenceParent")]
        public CanChangeSoftwareLicenceParentOutModel CanChangeSoftwareLicenceParent(CanSoftwareLicenceChangeParentInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new CanChangeSoftwareLicenceParentOutModel() { CanChange = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.CanChangeSoftwareLicenceParent userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = SoftwareLicence.CanChangeParent(model.ID, model.ParentID, dataSource);
                    return new CanChangeSoftwareLicenceParentOutModel() { CanChange = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "CanChangeSoftwareLicenceParent is NULL, id: '{0}'", model.ID);
                return new CanChangeSoftwareLicenceParentOutModel() { CanChange = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CanChangeSoftwareLicenceParent, id: '{0}'", model.ID);
                return new CanChangeSoftwareLicenceParentOutModel() { CanChange = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetLicenceTypeList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLicenceTypeList", Name = "GetLicenceTypeList")]
        public List<DTL.SimpleEnum> GetLicenceTypeList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("AssetApiController.GetLicenceTypeList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("AssetApiController.GetLicenceTypeList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = ContractLicence.GetLicenceTypeList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка типов лицензий.");
                return null;
            }
        }
        #endregion

        #region method GetLicenceSchemeList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLicenceSchemeList", Name = "GetLicenceSchemeList")]
        public List<DTL.SimpleEnum> GetLicenceSchemeList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("AssetApiController.GetLicenceSchemeList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("AssetApiController.GetLicenceSchemeList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = ContractLicence.GetLicenceSchemeList(HttpContext?.GetCurrentCulture() ?? CultureInfo.CurrentCulture.Name);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка схем лицензирования.");
                return null;
            }
        }
        #endregion

        #region method GetLicenceTypeForSoftwareLicenceList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLicenceTypeForSoftwareLicenceList", Name = "GetLicenceTypeForSoftwareLicenceList")]
        public List<DTL.SimpleEnum> GetLicenceTypeForSoftwareLicenceList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("AssetApiController.GetLicenceTypeForSoftwareLicenceList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("AssetApiController.GetLicenceTypeForSoftwareLicenceList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareLicence.GetLicenceTypeList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка типов лицензий.");
                return null;
            }
        }
        #endregion

        #region method GetLicenceSchemeForSoftwareLicenceList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLicenceSchemeForSoftwareLicenceList", Name = "GetLicenceSchemeForSoftwareLicenceList")]
        public List<DTL.SimpleEnum> GetLicenceSchemeForSoftwareLicenceList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("AssetApiController.GetLicenceSchemeForSoftwareLicenceList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("AssetApiController.GetLicenceSchemeForSoftwareLicenceList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareLicence.GetLicenceSchemeList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка схем лицензирования.");
                return null;
            }
        }
        #endregion

        #region method GetLicenceSchemeForSoftwareLicenceList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLicenceModelTemplateList", Name = "GetLicenceModelTemplateList")]
        public List<DTL.SimpleEnum> GetLicenceModelTemplateList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("AssetApiController.GetLicenceSchemeForSoftwareLicenceList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("AssetApiController.GetLicenceSchemeForSoftwareLicenceList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareLicence.GetLicenceModelTemplateList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка характера лицензии.");
                return null;
            }
        }
        #endregion

        #region SnapSoftwareLicenceSerialKey
        public sealed class SnapSoftwareLicenceSerialKeyModel
        {
            public Guid SoftwareLicenceID { get; set; }
            public Guid ObjectID { get; set; }
            public Guid SerialNumberID { get; set; }
        }
        public sealed class SnapSoftwareLicenceSerialKeyOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SnapSoftwareLicenceSerialKey", Name = "SnapSoftwareLicenceSerialKey")]
        public SnapSoftwareLicenceSerialKeyOutModel SnapSoftwareLicenceSerialKey(SnapSoftwareLicenceSerialKeyModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new SnapSoftwareLicenceSerialKeyOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.SnapSoftwareLicenceSerialKey userID={0}, userName={1}, SoftwareLicenceID={2}, SerialNumber={3}",
            user.Id, user.UserName, model.SoftwareLicenceID, model.SoftwareLicenceID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    SoftwareLicence.SnapSerialNumber(model.SoftwareLicenceID, model.ObjectID, model.SerialNumberID, dataSource);
                    dataSource.CommitTransaction();
                    return new SnapSoftwareLicenceSerialKeyOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"SnapSoftwareLicenceSerialKey concurency error. ID: '{0}'",
                        model.SoftwareLicenceID.ToString()));
                return new SnapSoftwareLicenceSerialKeyOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"SnapSoftwareLicenceSerialKey OBJECT concurency error. ID: '{0}'",
                        model.SoftwareLicenceID.ToString()));
                return new SnapSoftwareLicenceSerialKeyOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"SnapSoftwareLicenceSerialKey object deleted error. ID: '{0}'",
                        model.SoftwareLicenceID.ToString()));
                return new SnapSoftwareLicenceSerialKeyOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new SnapSoftwareLicenceSerialKeyOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion


        #region method AddSoftwareLicenceSerialNumber
        public sealed class AddSoftwareLicenceSerialNumberModel
        {
            public Guid SoftwareLicenceID { get; set; }
            public string SerialNumber { get; set; }
        }
        public sealed class AddSoftwareLicenceSerialNumberOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddSoftwareLicenceSerialNumber", Name = "AddSoftwareLicenceSerialNumber")]
        public AddSoftwareLicenceSerialNumberOutModel AddSoftwareLicenceSerialNumber(AddSoftwareLicenceSerialNumberModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddSoftwareLicenceSerialNumberOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.AddSoftwareLicenceSerialNumber userID={0}, userName={1}, SoftwareLicenceID={2}, SerialNumber={3}",
            user.Id, user.UserName, model.SoftwareLicenceID, model.SoftwareLicenceID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    SoftwareLicence.AddSerialNumber(model.SoftwareLicenceID, model.SerialNumber, dataSource);
                    dataSource.CommitTransaction();
                    return new AddSoftwareLicenceSerialNumberOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddSoftwareLicenceSerialNumber concurency error. ID: '{0}'",
                        model.SoftwareLicenceID.ToString()));
                return new AddSoftwareLicenceSerialNumberOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddSoftwareLicenceSerialNumber OBJECT concurency error. ID: '{0}'",
                        model.SoftwareLicenceID.ToString()));
                return new AddSoftwareLicenceSerialNumberOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"AddSoftwareLicenceSerialNumber object deleted error. ID: '{0}'",
                        model.SoftwareLicenceID.ToString()));
                return new AddSoftwareLicenceSerialNumberOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddSoftwareLicenceSerialNumberOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method AddInventorySpecification
        public sealed class AddInventorySpecificationInputModel
        {
            public List<DTL.ObjectInfo> DependencyList { get; set; }
            public Guid WorkOrderID { get; set; }
        }
        public sealed class AddInventorySpecificationOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddInventorySpecification", Name = "AddInventorySpecification")]
        public AddInventorySpecificationOutModel AddInventorySpecification(AddInventorySpecificationInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddInventorySpecificationOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.AddInventorySpecification userID={0}, userName={1}, ReferenceListCount={2}, WorkOrderID={3}",
            user.Id, user.UserName, model.DependencyList.Count, model.WorkOrderID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    InventorySpecification.AddSpecification(model.WorkOrderID, model.DependencyList, dataSource, user.User);
                    //
                    dataSource.CommitTransaction();
                    return new AddInventorySpecificationOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddInventorySpecification concurency error. ID: '{0}'. Assets: '{1}'",
                        model.WorkOrderID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddInventorySpecificationOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddInventorySpecification OBJECT concurency error. ID: '{0}'. Assets: '{1}'",
                        model.WorkOrderID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddInventorySpecificationOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"AddInventorySpecification object deleted error. ID: '{0}'. Assets: '{1}'",
                        model.WorkOrderID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddInventorySpecificationOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddInventorySpecificationOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method GetInventorySpecification
        public sealed class GetInventorySpecificationOutModel
        {
            public InventorySpecification InventorySpecification { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetInventorySpecificationInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetInventorySpecification", Name = "GetInventorySpecification")]
        public GetInventorySpecificationOutModel GetInventorySpecification(GetInventorySpecificationInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetInventorySpecificationOutModel() { InventorySpecification = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetInventorySpecification userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_NETWORKDEVICE))
                {
                    Logger.Trace("SDApiController.GetInventorySpecification userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetInventorySpecificationOutModel() { InventorySpecification = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = InventorySpecification.Get(model.ID, user.User.ID, dataSource);
                    return new GetInventorySpecificationOutModel() { InventorySpecification = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetInventorySpecification is NULL, id: '{0}'", model.ID);
                return new GetInventorySpecificationOutModel() { InventorySpecification = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetInventorySpecification, id: '{0}'", model.ID);
                return new GetInventorySpecificationOutModel() { InventorySpecification = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region InventoryConfirm
        public sealed class InventoryConfirmInfo
        {
            public List<DTL.ObjectInfo> InventorySpecificationList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/InventoryConfirm", Name = "InventoryConfirm")]
        public AssetOperationOutModel InventoryConfirm(InventoryConfirmInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.InventoryConfirm userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    InventorySpecification.Confirm(model.InventorySpecificationList, dataSource);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"InventoryConfirm not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "InventoryConfirm, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region InventoryIgnore
        public sealed class InventoryIgnoreInfo
        {
            public List<DTL.ObjectInfo> InventorySpecificationList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/InventoryIgnore", Name = "InventoryIgnore")]
        public AssetOperationOutModel InventoryIgnore(InventoryIgnoreInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.InventoryIgnore userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    InventorySpecification.Ignore(model.InventorySpecificationList, dataSource);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"InventoryIgnore not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "InventoryIgnore, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region InventoryUpdateDB
        public sealed class InventoryUpdateDBInfo
        {
            public List<InventorySpecificationInfo> List { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/InventoryUpdateDB", Name = "InventoryUpdateDB")]
        public AssetOperationOutModel InventoryUpdateDB(InventoryUpdateDBInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.InventoryUpdateDB userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    InventorySpecification.UpdateDB(model.List, dataSource);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"InventoryUpdateDB not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "InventoryUpdateDB, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region InventoryOperationExecuted
        public sealed class InventoryOperationExecutedInfo
        {
            public List<DTL.ObjectInfo> InventorySpecificationList { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/InventoryOperationExecuted", Name = "InventoryOperationExecuted")]
        public AssetOperationOutModel InventoryOperationExecuted(InventoryOperationExecutedInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new AssetOperationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.InventoryOperationExecuted userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    InventorySpecification.OperationExecuted(model.InventorySpecificationList, dataSource);
                    //
                    return new AssetOperationOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"InventoryOperationExecuted not supported, model: '{0}'", model));
                return new AssetOperationOutModel() { Result = RequestResponceType.OperationError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "InventoryOperationExecuted, model: {0}.", model);
                return new AssetOperationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region HasStorageLocation
        public sealed class HasStorageLocationOutModel
        {
            public bool Retval { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/HasStorageLocation", Name = "HasStorageLocation")]
        public HasStorageLocationOutModel HasStorageLocation()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new HasStorageLocationOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.HasStorageLocation userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = StorageLocation.HasStorageLocation(dataSource);
                    //
                    return new HasStorageLocationOutModel() { Retval = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "HasStorageLocation");
                return new HasStorageLocationOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetDisabledOperationsWF
        public sealed class GetDisabledOperationsWFOutModel
        {
            public List<int> Retval { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetDisabledOperationsWF", Name = "GetDisabledOperationsWF")]
        public GetDisabledOperationsWFOutModel GetDisabledOperationsWF()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new GetDisabledOperationsWFOutModel() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("AssetApiController.GetDisabledOperationsWF userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = OperationHelper.GetDisabledOperationsWF(dataSource);
                    //
                    return new GetDisabledOperationsWFOutModel() { Retval = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetDisabledOperationsWF");
                return new GetDisabledOperationsWFOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region GetConfigurationUnit
        public sealed class GetConfigurationUnitOutModel
        {
            public ConfigurationUnit ConfigurationUnit { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetConfigurationUnitInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetConfigurationUnit", Name = "GetConfigurationUnit")]
        public GetConfigurationUnitOutModel GetConfigurationUnit(GetConfigurationUnitInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetConfigurationUnitOutModel() { ConfigurationUnit = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetConfigurationUnit userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ConfigurationUnit_Properties))
                {
                    Logger.Trace("AssetApiController.GetConfigurationUnit userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetConfigurationUnitOutModel() { ConfigurationUnit = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ConfigurationUnit.Get(model.ID, user.User.ID, dataSource);
                    return new GetConfigurationUnitOutModel() { ConfigurationUnit = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetConfigurationUnit is NULL, id: '{0}'", model.ID);
                return new GetConfigurationUnitOutModel() { ConfigurationUnit = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetConfigurationUnit, id: '{0}'", model.ID);
                return new GetConfigurationUnitOutModel() { ConfigurationUnit = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region GetCluster
        public sealed class GetClusterOutModel
        {
            public Cluster Cluster { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetClusterInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetCluster", Name = "GetCluster")]
        public GetClusterOutModel GetCluster(GetClusterInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetClusterOutModel() { Cluster = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetCluster userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Cluster_Properties))
                {
                    Logger.Trace("AssetApiController.GetCluster userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetClusterOutModel() { Cluster = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Cluster.Get(model.ID, user.User.ID, dataSource);
                    return new GetClusterOutModel() { Cluster = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetCluster is NULL, id: '{0}'", model.ID);
                return new GetClusterOutModel() { Cluster = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCluster, id: '{0}'", model.ID);
                return new GetClusterOutModel() { Cluster = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AddCluster
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddCluster", Name = "AddCluster")]
        public GetClusterOutModel AddCluster(DTL.Assets.Cluster info)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetClusterOutModel() { Cluster = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.AddCluster userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Cluster_Add))
                {
                    Logger.Trace("AssetApiController.AddCluster userID={0}, userName={1} (operation denied)", user.Id, user.UserName);
                    return new GetClusterOutModel() { Cluster = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = Cluster.Add(info, dataSource);
                    //
                    return new GetClusterOutModel() { Cluster = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddCluster");
                return new GetClusterOutModel() { Cluster = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region CreateConfigurationUnit
        public sealed class CreateConfigurationUnitOutModel
        {
            public Core.BaseObject ConfigurationUnit { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/CreateConfigurationUnit", Name = "CreateConfigurationUnit")]
        public CreateConfigurationUnitOutModel CreateConfigurationUnit(DTL.Assets.ConfigurationUnit info)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new CreateConfigurationUnitOutModel() { ConfigurationUnit = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.CreateConfigurationUnit userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Cluster_Add))
                {
                    Logger.Trace("AssetApiController.CreateConfigurationUnit userID={0}, userName={1} (operation denied)", user.Id, user.UserName);
                    return new CreateConfigurationUnitOutModel() { ConfigurationUnit = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = ConfigurationUnit.CreateConfigurationUnit(info, dataSource);
                    //
                    return new CreateConfigurationUnitOutModel() { ConfigurationUnit = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CreateConfigurationUnit");
                return new CreateConfigurationUnitOutModel() { ConfigurationUnit = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetLogicalObject
        public sealed class GetLogicalObjectOutModel
        {
            public LogicalObject LogicalObject { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetLogicalObjectInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLogicalObject", Name = "GetLogicalObject")]
        public GetLogicalObjectOutModel GetLogicalObject(GetLogicalObjectInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetLogicalObjectOutModel() { LogicalObject = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetCluster userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Cluster_Properties))
                {
                    Logger.Trace("AssetApiController.GetCluster userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetLogicalObjectOutModel() { LogicalObject = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = LogicalObject.Get(model.ID, user.User.ID, dataSource);
                    return new GetLogicalObjectOutModel() { LogicalObject = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetCluster is NULL, id: '{0}'", model.ID);
                return new GetLogicalObjectOutModel() { LogicalObject = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCluster, id: '{0}'", model.ID);
                return new GetLogicalObjectOutModel() { LogicalObject = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AddLogicalObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddLogicalObject", Name = "AddLogicalObject")]
        public GetLogicalObjectOutModel AddLogicalObject(DTL.Assets.LogicalObjectRegister info)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetLogicalObjectOutModel() { LogicalObject = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.AddLogicalObject userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_Cluster_Add))
                {
                    Logger.Trace("AssetApiController.AddLogicalObject userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName);
                    return new GetLogicalObjectOutModel() { LogicalObject = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = LogicalObject.Add(info, dataSource);
                    //
                    return new GetLogicalObjectOutModel() { LogicalObject = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddLogicalObject");
                return new GetLogicalObjectOutModel() { LogicalObject = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetDataEntityObject
        public sealed class GetDataEntityObjectOutModel
        {
            public DataEntityObject DataEntityObject { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetDataEntityObjectInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetDataEntityObject", Name = "GetDataEntityObject")]
        public GetDataEntityObjectOutModel GetDataEntityObject(GetDataEntityObjectInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetDataEntityObjectOutModel() { DataEntityObject = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetDataEntityObject userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_DataEntity_Properties))
                {
                    Logger.Trace("AssetApiController.GetDataEntityObject userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new GetDataEntityObjectOutModel() { DataEntityObject = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = DataEntityObject.Get(model.ID, user.User.ID, dataSource);
                    return new GetDataEntityObjectOutModel() { DataEntityObject = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetDataEntityObject is NULL, id: '{0}'", model.ID);
                return new GetDataEntityObjectOutModel() { DataEntityObject = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetDataEntityObject, id: '{0}'", model.ID);
                return new GetDataEntityObjectOutModel() { DataEntityObject = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AddDataEntityObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddDataEntityObject", Name = "AddDataEntityObject")]
        public GetDataEntityObjectOutModel AddDataEntityObject(DTL.Assets.DataEntityObjectRegister info)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetDataEntityObjectOutModel() { DataEntityObject = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.AddDataEntityObject userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_DataEntity_Add))
                {
                    Logger.Trace("AssetApiController.AddDataEntityObject userID={0}, userName={1} (operation denied)", user.Id, user.UserName);
                    return new GetDataEntityObjectOutModel() { DataEntityObject = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = DataEntityObject.Add(info, dataSource);
                    //
                    return new GetDataEntityObjectOutModel() { DataEntityObject = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddDataEntityObject");
                return new GetDataEntityObjectOutModel() { DataEntityObject = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region CreateConfigurationUnitsByObjectsOutModel

        public sealed class ObjectListModel
        {
            public List<ObjectInfo> ObjectList { get; set; }
        }

        public sealed class CreateConfigurationUnitsByObjectsOutModel
        {
            public string Error { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("POST")]
        [Route("assetApi/CreateConfigurationUnitsByObjects", Name = "CreateConfigurationUnitsByObjects")]
        public CreateConfigurationUnitsByObjectsOutModel CreateConfigurationUnitsByObjects(ObjectListModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new CreateConfigurationUnitsByObjectsOutModel() { Error = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.CreateConfigurationUnitsByObjects userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ConfigurationUnit_Add))
                {
                    Logger.Trace("AssetApiController.CreateConfigurationUnitsByObjects userID={0}, userName={1} (operation denied)", user.Id, user.UserName);
                    return new CreateConfigurationUnitsByObjectsOutModel() { Error = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    string error = ConfigurationUnit.CreateByObjects(model.ObjectList, dataSource);
                    //
                    return new CreateConfigurationUnitsByObjectsOutModel() { Error = error, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CreateConfigurationUnitsByObjects");
                return new CreateConfigurationUnitsByObjectsOutModel() { Error = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region CanIncludeToInfrastructure
        [HttpGet]
        [AcceptVerbs("POST")]
        [Route("assetApi/CanIncludeToInfrastructure", Name = "CanIncludeToInfrastructure")]
        public bool CanIncludeToInfrastructure(ObjectListModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return false;
            //
            if (model == null || model.ObjectList == null)
                return false;
            //
            Logger.Trace("AssetApiController.CanIncludeToInfrastructure userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    return ConfigurationUnit.CanIncludeToInfrastructure(model.ObjectList, user.User.ID, dataSource);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "CanIncludeToInfrastructure");
                return false;
            }
        }
        #endregion


        #region GetSubdeviceParameterList

        public sealed class GetCriticalityListOutModel
        {
            public List<Criticality> CriticalityList { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetCriticalityList", Name = "GetCriticalityList")]
        public GetCriticalityListOutModel GetCriticalityList()
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetCriticalityListOutModel() { CriticalityList = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("sdApi/GetCriticalityList userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<Criticality> retval;
                    retval = Asset.GetCriticalityLIst(dataSource);
                    //

                    return new GetCriticalityListOutModel() { CriticalityList = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetCriticalityList is NULL");
                return new GetCriticalityListOutModel() { CriticalityList = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCriticalityList");
                return new GetCriticalityListOutModel() { CriticalityList = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetConfigurationUnitTypeList
        public sealed class GetConfigurationUnitTypeListOutModel
        {
            public InvoiceHelper[] List { get; set; }
            //public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetConfigurationUnitTypeList", Name = "GetConfigurationUnitTypeList")]
        public List<InvoiceHelper> GetConfigurationUnitTypeList([FromQuery] bool onlyHosts, [FromQuery] Guid? deviceID, [FromQuery] int? deviceClassID)
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("FinanceApiController.GetConfigurationUnitTypeList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("FinanceApiController.GetConfigurationUnitTypeList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = InvoiceHelper.GetConfigurationUnitTypeList(onlyHosts, deviceID, deviceClassID);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка типов конфигурационных единиц.");
                return null;
            }
        }
        #endregion
        //
        #region GetLogicalObjectSettings
        public sealed class GetLogicalObjectSettingsModel
        {
            public bool EditIPAddressName { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLogicalObjectSettings", Name = "GetLogicalObjectSettings")]
        public GetLogicalObjectSettingsModel GetLogicalObjectSettings()
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetLogicalObjectSettingsModel() { EditIPAddressName = false, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetLogicalObjectSettings userID={0}, userName={1}, ID={2}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = InfraManager.IM.BusinessLayer.Settings.Setting.Get(InfraManager.IM.BusinessLayer.Settings.SettingType.AutoCreateNetworkUnit).GetBoolean();
                    return new GetLogicalObjectSettingsModel() { EditIPAddressName = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetLogicalObjectSettings");
                return new GetLogicalObjectSettingsModel() { EditIPAddressName = false, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetLogicalObjectSettings");
                return new GetLogicalObjectSettingsModel() { EditIPAddressName = false, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method AddContractMaintenance
        public sealed class AddClusterHostsInputModel
        {
            public List<DTL.ObjectInfo> DependencyList { get; set; }
            public Guid ClusterID { get; set; }
        }
        public sealed class AddClusterHostsOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddClusterHosts", Name = "AddClusterHosts")]
        public AddClusterHostsOutModel AddClusterHosts(AddClusterHostsInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddClusterHostsOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.AddClusterHosts userID={0}, userName={1}, ReferenceListCount={2}, ClusterID={3}",
            user.Id, user.UserName, model.DependencyList.Count, model.ClusterID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    //
                    Cluster.AddHostsReference(model.ClusterID, model.DependencyList, dataSource);
                    //
                    dataSource.CommitTransaction();
                    return new AddClusterHostsOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddClusterHosts concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ClusterID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddClusterHostsOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddClusterHosts OBJECT concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ClusterID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddClusterHostsOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"AddClusterHosts object deleted error. ID: '{0}'. Assets: '{1}'",
                        model.ClusterID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddClusterHostsOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddClusterHostsOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region GetLogicalObject
        public sealed class GetTypeIDInOut
        {
            public TypeOut Type { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetTypeIDIn
        {
            public Guid ID { get; set; }
        }
        public sealed class TypeOut
        {
            public string Name { get; set; }
            public string ClassID { get; set; }
            public string ID { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetType", Name = "GetType")]
        public GetTypeIDInOut GetType([FromQuery] GetTypeIDIn model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetTypeIDInOut() { Type = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetType userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = InfraManager.IM.BusinessLayer.ProductCatalogue.ProductCatalogType.Get(model.ID, dataSource);
                    TypeOut outType = new TypeOut();
                    outType.ClassID = retval.Template.ClassID.ToString();
                    outType.Name = retval.Name;
                    outType.ID = retval.ID.ToString();
                    return new GetTypeIDInOut() { Type = outType, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetCluster is NULL, id: '{0}'", model.ID);
                return new GetTypeIDInOut() { Type = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetCluster, id: '{0}'", model.ID);
                return new GetTypeIDInOut() { Type = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion
        //
        #region method AddClusterOrHostVMsInputModel
        public sealed class AddClusterOrHostVMInputModel
        {
            public List<DTL.ObjectInfo> DependencyList { get; set; }
            public int ClassID { get; set; }
            public String Name { get; set; }
            public Guid ID { get; set; }
        }
        public sealed class AddClusterOrHostVMOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddClusterVM", Name = "AddClusterOrHostVM")]
        public AddClusterOrHostVMOutModel AddClusterOrHostVM(AddClusterOrHostVMInputModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddClusterOrHostVMOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.AddClusterHosts userID={0}, userName={1}, ReferenceListCount={2}, ClusterID={3}",
            user.Id, user.UserName, model.DependencyList.Count, model.ID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    //
                    LogicalObject.SetHostDevice(model.ID, model.ClassID, model.Name, model.DependencyList, dataSource);
                    //
                    dataSource.CommitTransaction();
                    return new AddClusterOrHostVMOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (FieldConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddClusterVM concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddClusterOrHostVMOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectConcurrencyException e)
            {
                Logger.Error(e, string.Format(@"AddClusterVM OBJECT concurency error. ID: '{0}'. Assets: '{1}'",
                        model.ID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddClusterOrHostVMOutModel() { Result = (RequestResponceType.ConcurrencyError) };
            }
            catch (ObjectDeletedException e)
            {
                Logger.Error(e, string.Format(@"AddClusterVM object deleted error. ID: '{0}'. Assets: '{1}'",
                        model.ID.ToString(),
                        string.Join(@"\n, ", model.DependencyList.Select(d => d.ID.ToString()))));
                return new AddClusterOrHostVMOutModel() { Result = (RequestResponceType.ObjectDeleted) };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении связей.");
                return new AddClusterOrHostVMOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion



        #region method SaveSoftwareModelUsingTypeObject

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveSoftwareModelUsingTypeObject", Name = "SaveSoftwareModelUsingTypeObject")]
        public GeneralCatalogSaveOutModel SaveSoftwareModelUsingTypeObject([FromBody] DTL.Software.SoftwareModelUsingType model)
        {
            var softwareModelUsingType = new BLL.Software.SoftwareModelUsingType.SoftwareModelUsingTypeGeneral();
            //
            return GeneralCatalogSave(softwareModelUsingType, model, "SaveSoftwareModelUsingTypeObject");
        }
        #endregion

        #region method GetSoftwareModelUsingTypeObjectById
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelUsingTypeObjectById", Name = "GetSoftwareModelUsingTypeObjectById")]
        public GeneralGetCatalogByIdOutModel GetSoftwareModelUsingTypeObjectById([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var softwareModelUsingType = new BLL.Software.SoftwareModelUsingType.SoftwareModelUsingTypeGeneral();
            //
            return GeneralGetCatalogById(softwareModelUsingType, model.ID, "GetSoftwareModelUsingTypeObjectById");
        }
        #endregion

        #region method RemoveSoftwareModelUsingTypeObject

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveSoftwareModelUsingTypeObject", Name = "RemoveSoftwareModelUsingTypeObject")]
        public RemoveGeneralOutModel RemoveSoftwareModelUsingTypeObject([FromBody] List<ListInfoWithRowVersion> model)
        {
            var softwareModelUsingType = new BLL.Software.SoftwareModelUsingType.SoftwareModelUsingTypeGeneral();
            //
            return GeneralCatalogRemove(softwareModelUsingType, model, "RemoveSoftwareModelUsingTypeObject");
        }
        #endregion

        #region method SetDefaultSoftwareModelUsingType
        public sealed class SetDefaultSoftwareModelUsingTypeInModel
        {
            public Guid ID { get; set; }
            public string RowVersion { get; set; }
        }

        public sealed class SetDefaultSoftwareModelUsingTypeOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SetDefaultSoftwareModelUsingType", Name = "SetDefaultSoftwareModelUsingType")]
        public SetDefaultSoftwareModelUsingTypeOutModel SetDefaultSoftwareModelUsingType([FromBody] SetDefaultSoftwareModelUsingTypeInModel model)
        {
            var user = base.CurrentUser;
            try
            {
                Logger.Trace($"AssetApiController.SetDefaultSoftwareModelUsingType userID={user.Id}, userName={user.UserName}, ID={model.ID}");
                using (var dataSource = DataSource.GetDataSource())
                {
                    BLL.Software.SoftwareModelUsingType.SetDefault(model.ID, model.RowVersion, dataSource);
                }
                return new SetDefaultSoftwareModelUsingTypeOutModel() { Result = RequestResponceType.Success };
            }
            catch (ObjectConcurrencyException)
            {
                return new SetDefaultSoftwareModelUsingTypeOutModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при установлении флага IsDefault");
                return new SetDefaultSoftwareModelUsingTypeOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method RemoveManufacturer

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveManufacturer", Name = "RemoveManufacturer")]
        public RemoveGeneralOutModel RemoveManufacturer([FromBody] List<ListInfoWithRowVersion> model)
        {
            var manufacturer = new BLL.Assets.Manufacturer.ManufacturerGeneral();
            //
            return GeneralCatalogRemove(manufacturer, model, "RemoveManufacturer");
        }
        #endregion

        #region method SaveSaveManufacturer


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveManufacturer", Name = "SaveManufacturer")]
        public GeneralCatalogSaveOutModel SaveSaveManufacturer([FromBody] DTL.Assets.Manufacturer model)
        {
            var manufacturer = new BLL.Assets.Manufacturer.ManufacturerGeneral();
            //
            return GeneralCatalogSave(manufacturer, model, "SaveManufacturer");
        }
        #endregion

        #region method GetManufacturerById
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetManufacturerById", Name = "GetManufacturerById")]
        public GeneralGetCatalogByIdOutModel GetManufacturerById([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var manufacturer = new BLL.Assets.Manufacturer.ManufacturerGeneral();
            //
            return GeneralGetCatalogById(manufacturer, model.ID, "GetManufacturerById");
        }
        #endregion

        #region method NominateSynonym
        public sealed class NominateSynonymOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        public class SynonymChangeModel
        {
            public string oldID { get; set; }
            public string newID { get; set; }
        }

        public sealed class NominateSynonymInModel
        {
            public List<SynonymChangeModel> Synonyms { get; set; }
            public string TableName { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/NominateSynonyms", Name = "NominateSynonyms")]
        public NominateSynonymOutModel NominateSynonym([FromBody] NominateSynonymInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new NominateSynonymOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.NominateSynonyms userID={0}, userName={1}, tableName={2}",
            user.Id, user.UserName, model.TableName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    int? directoryID;
                    BLL.Synonyms.Synonym.CanUseTableForSynonyms(model.TableName, dataSource, out directoryID);
                    if (!directoryID.HasValue)
                    {
                        Logger.Trace($"Для данной таблицы синонимы не включены");
                        return new NominateSynonymOutModel() { Result = (RequestResponceType.SynonymNotEnabled) };
                    }
                    //
                    foreach (var el in model.Synonyms)
                    {
                        BLL.Synonyms.Synonym.UpdateLinks(model.TableName, el.oldID, el.newID, directoryID.Value, dataSource);
                    }
                    //
                    return new NominateSynonymOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении синонима");
                return new NominateSynonymOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method UnnominateSynonym
        public sealed class UnNominateSynonymsOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        public sealed class UnnominateSynonymsInModel
        {
            public List<string> SynonymsID { get; set; } // Guid
            public string TableName { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/UnNominateSynonyms", Name = "UnNominateSynonyms")]
        public UnNominateSynonymsOutModel UnNominateSynonyms([FromBody] UnnominateSynonymsInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new UnNominateSynonymsOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace("AssetApiController.UnNominateSynonyms userID={0}, userName={1}, tableName={2}",
            user.Id, user.UserName, model.TableName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    int? directoryID;
                    BLL.Synonyms.Synonym.CanUseTableForSynonyms(model.TableName, dataSource, out directoryID);
                    if (!directoryID.HasValue)
                    {
                        Logger.Trace($"Для данной таблицы синонимы не включены");
                        return new UnNominateSynonymsOutModel() { Result = (RequestResponceType.SynonymNotEnabled) };
                    }
                    //
                    foreach (var el in model.SynonymsID)
                    {
                        BLL.Synonyms.Synonym.UnNominate(model.TableName, el, dataSource);
                    }
                    //
                    return new UnNominateSynonymsOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении синонима");
                return new UnNominateSynonymsOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion




        #region method GetSynonyms
        public sealed class GetSynonymsOutModel
        {
            public List<DTL.Synonym.SynonymValue> Synonym { get; set; }
            public RequestResponceType Result { get; set; }
        }
        public sealed class GetSynonymsInModel
        {
            public string TrueID { get; set; }
            public string TableName { get; set; }
            public string ParameterValue { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSynonyms", Name = "GetSynonyms")]
        public GetSynonymsOutModel GetSynonyms([FromBody] GetSynonymsInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GetSynonymsOutModel() { Result = (RequestResponceType.NullParamsError), Synonym = null };
            //
            Logger.Trace($"AssetApiController.GetSynonyms userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    int? directoryID;
                    BLL.Synonyms.Synonym.CanUseTableForSynonyms(model.TableName, dataSource, out directoryID);
                    if (!directoryID.HasValue)
                    {
                        Logger.Trace($"Для данной таблицы синонимы не включены");
                        return new GetSynonymsOutModel() { Result = (RequestResponceType.SynonymNotEnabled), Synonym = null };
                    }
                    //
                    return new GetSynonymsOutModel() { Result = RequestResponceType.Success, Synonym = BLL.Synonyms.Synonym.GetSynonyms(model.TrueID, directoryID.Value, model.ParameterValue, dataSource) };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении синонимов.");
                return new GetSynonymsOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method GetTrueIdByValue
        public sealed class GetTrueIdByValueOutModel
        {
            public string TrueID { get; set; }
            public RequestResponceType Result { get; set; }
        }
        public sealed class GetTrueIdByValueInModel
        {
            public string TableName { get; set; }
            public string ParameterValue { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSynonymTrueIDByValue", Name = "GetSynonymTrueIDByValue")]
        public GetTrueIdByValueOutModel GetSynonymTrueIDByValue([FromBody] GetTrueIdByValueInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GetTrueIdByValueOutModel() { Result = (RequestResponceType.NullParamsError), TrueID = null };
            //
            Logger.Trace($"AssetApiController.GetSynonyms userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    int? directoryID;
                    BLL.Synonyms.Synonym.CanUseTableForSynonyms(model.TableName, dataSource, out directoryID);
                    if (!directoryID.HasValue)
                    {
                        Logger.Trace($"Для данной таблицы синонимы не включены.");
                        return new GetTrueIdByValueOutModel() { Result = (RequestResponceType.SynonymNotEnabled), TrueID = null }; //change
                    }
                    //
                    return new GetTrueIdByValueOutModel() { Result = RequestResponceType.Success, TrueID = BLL.Synonyms.Synonym.GetTrueIdByValue(model.ParameterValue, directoryID.Value, dataSource) };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении TrueID.");
                return new GetTrueIdByValueOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion  

        #region method GetManufacturersExcludingThisID
        public sealed class GetManufacturersExcludingThisIDOutModel
        {
            public List<DTL.Assets.Manufacturer> Manufacturer { get; set; }
            public RequestResponceType Result { get; set; }
        }
        public sealed class GetManufacturersExcludingThisIDInModel
        {
            public string TableName { get; set; }
            public Guid ExceptID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetManufacturersExcludingThisID", Name = "GetManufacturersExcludingThisID")]
        public GetManufacturersExcludingThisIDOutModel GetManufacturersExcludingThisID([FromBody] GetManufacturersExcludingThisIDInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GetManufacturersExcludingThisIDOutModel() { Result = (RequestResponceType.NullParamsError), Manufacturer = null };
            //
            Logger.Trace($"AssetApiController.GetManufacturersExcludingThisID userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    return new GetManufacturersExcludingThisIDOutModel() { Result = RequestResponceType.Success, Manufacturer = BLL.Assets.Manufacturer.GetManufacturersExcludingThisID(model.ExceptID, dataSource) };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении синонимов.");
                return new GetManufacturersExcludingThisIDOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion


        #region method GeneralGetCatalogById
        public sealed class GeneralGetCatalogByIdInModel
        {
            public Guid ID { get; set; }
        }

        public sealed class GeneralGetModelOut<T> where T : class
        {
            public RequestResponceType Result { get; set; }
            public T Catalog { get; set; }
        }

        public sealed class GeneralGetCatalogByIdOutModel
        {
            public DTL.Assets.CatalogGeneral Catalog { get; set; }
            public RequestResponceType Result { get; set; }
        }
        public GeneralGetCatalogByIdOutModel GeneralGetCatalogById(ICatalogGeneral catalogGeneral, Guid id, string friendlyCatalogNameForTrace)
        {
            var user = base.CurrentUser;
            if (user == null || id == Guid.Empty)
                return new GeneralGetCatalogByIdOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace($"AssetApiController.{friendlyCatalogNameForTrace} userID={user.Id}, userName={user.UserName}, ID={id}");
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    DTL.Assets.CatalogGeneral catalog = catalogGeneral.Get(id, user.User.ID, dataSource);
                    //
                    return new GeneralGetCatalogByIdOutModel() { Result = (RequestResponceType.Success), Catalog = catalog };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, $"Ошибка при получении справочника {friendlyCatalogNameForTrace}.");
                return new GeneralGetCatalogByIdOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при получении справочника {friendlyCatalogNameForTrace}.");
                return new GeneralGetCatalogByIdOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }

        #endregion

        #region method GeneralCatalogRemove

        public sealed class RemoveGeneralOutModel
        {
            public RequestResponceType Result { get; set; }
            public List<string> InUseList { get; set; }
            public List<string> ConcurrencyList { get; set; }
        }
        private RemoveGeneralOutModel GeneralCatalogRemove(ICatalogGeneral catalog, List<ListInfoWithRowVersion> model, string friendlyCatalogNameForTrace)
        {
            var user = base.CurrentUser;
            if (model == null || model.Count < 1)
                return new RemoveGeneralOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace($"AssetApiController.{friendlyCatalogNameForTrace} userID={0}, userName={1}",
            user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<string> InUseList = new List<string>();
                    List<string> ConcurrencyList = new List<string>();
                    foreach (var el in model)
                    {
                        try
                        {
                            catalog.Remove(el, dataSource);
                        }
                        catch(ObjectInUseException)
                        {
                            InUseList.Add(el.Name);
                        }
                        catch (SqlException e)
                        {
                            if (e.Number == 547) // Foreign Key violation
                                InUseList.Add(el.Name);
                        }                       
                        catch (ObjectConcurrencyException)
                        {
                            ConcurrencyList.Add(el.Name);
                        }
                        catch(Exception)
                        {
                            InUseList.Add(el.Name);
                        }
                    }
                    //
                    return new RemoveGeneralOutModel() { Result = (RequestResponceType.Success), InUseList = InUseList, ConcurrencyList = ConcurrencyList };
                }
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, $"Ошибка при удалении справочника {friendlyCatalogNameForTrace}.");
                return new RemoveGeneralOutModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, $"Ошибка при удалении справочника {friendlyCatalogNameForTrace}.");
                return new RemoveGeneralOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при удалении справочника {friendlyCatalogNameForTrace}");
                return new RemoveGeneralOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method GeneralCatalogSave
        public sealed class GeneralCatalogSaveOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        public GeneralCatalogSaveOutModel GeneralCatalogSave(ICatalogGeneral catalog, DTL.Assets.CatalogGeneral model, string friendlyCatalogNameForTrace)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace($"AssetApiController.{friendlyCatalogNameForTrace} userID={user.Id}, userName={user.UserName}, ID={model.ID}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (catalog.ExistsByName(model, dataSource))
                    {
                        Logger.Trace($"Тип {friendlyCatalogNameForTrace} уже существует, имя {model.ID}");
                        return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.ExistsByName) };
                    }
                    catalog.Save(model, user.User.ID, dataSource);
                    //
                    return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, $"Ошибка при редактировании справочника {friendlyCatalogNameForTrace}.");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, $"Ошибка при редактировании справочника {friendlyCatalogNameForTrace}.");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при сохраннении справочника {friendlyCatalogNameForTrace}.");
                return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion


        #region method GetPositiontById
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetPositionById", Name = "GetPositionById")]
        public GeneralGetCatalogByIdOutModel GetPositiontById([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var position = new Position.PositionGeneral();
            return GeneralGetCatalogById(position, model.ID, "GetPositiontById");
        }
        #endregion

        #region method RemovePositions
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemovePositions", Name = "RemovePositions")]
        public RemoveGeneralOutModel RemovePositions([FromBody] List<ListInfoWithRowVersion> model)
        {
            var position = new Position.PositionGeneral();
            //
            return GeneralCatalogRemove(position, model, "RemovePositions");
        }
        #endregion

        #region method SavePosition

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SavePosition", Name = "SavePosition")]
        public GeneralCatalogSaveOutModel SavePosition([FromBody] DTL.Assets.Position model)
        {
            var position = new Position.PositionGeneral();
            //
            return GeneralCatalogSave(position, model, "SavePosition");
        }
        #endregion

        #region method GetPositionsExcludingThisID
        public sealed class GetPositionsExcludingThisIDOutModel
        {
            public List<DTL.Assets.Position> Position { get; set; }
            public RequestResponceType Result { get; set; }
        }
        public sealed class GetPositionsExcludingThisIDInModel
        {
            public string TableName { get; set; }
            public Guid ExceptID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetPositionsExcludingThisID", Name = "GetPositionsExcludingThisID")]
        public GetPositionsExcludingThisIDOutModel GetPositionsExcludingThisID([FromBody] GetPositionsExcludingThisIDInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GetPositionsExcludingThisIDOutModel() { Result = (RequestResponceType.NullParamsError), Position = null };
            //
            Logger.Trace($"AssetApiController.GetPositionsExcludingThisID userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    return new GetPositionsExcludingThisIDOutModel() { Result = RequestResponceType.Success, Position = BLL.Assets.Position.GetPositionsExcludingThisID(model.ExceptID, dataSource) };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении синонимов.");
                return new GetPositionsExcludingThisIDOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion


        #region method GetUnitById
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetUnitById", Name = "GetUnitById")]
        public GeneralGetCatalogByIdOutModel GetUnitById([FromBody] Guid id)
        {
            var unit = new BLL.Finance.Unit.Unit.UnitGeneral();
            //
            return GeneralGetCatalogById(unit, id, "GetUnitById");
        }
        #endregion

        #region method SaveUnit
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveUnit", Name = "SaveUnit")]
        public GeneralCatalogSaveOutModel SaveUnit([FromBody] DTL.Finance.Unit model)
        {
            var unit = new BLL.Finance.Unit.Unit.UnitGeneral();
            //
            return GeneralCatalogSave(unit, model, "SaveUnit");
        }
        #endregion

        #region method RemoveUnits

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveUnits", Name = "RemoveUnits")]
        public RemoveGeneralOutModel RemoveUnits([FromBody] List<ListInfoWithRowVersion> model)
        {
            var unit = new BLL.Finance.Unit.Unit.UnitGeneral();
            //
            return GeneralCatalogRemove(unit, model, "RemoveUnits");
        }
        #endregion


        #region method GetCriticalityById
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCriticalityById", Name = "GetCriticalityById")]
        public GeneralGetCatalogByIdOutModel GetCriticalityById([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var criticality = new BLL.Assets.Criticalities.CriticalitiesGeneral();
            return GeneralGetCatalogById(criticality, model.ID, "GetCriticalityById");
        }
        #endregion

        #region method SaveCriticality

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveCriticality", Name = "SaveCriticality")]
        public GeneralCatalogSaveOutModel SaveCriticality([FromBody] DTL.Assets.Criticality model)
        {

            var criticality = new BLL.Assets.Criticalities.CriticalitiesGeneral();
            //
            return GeneralCatalogSave(criticality, model, "SaveCriticality");
        }
        #endregion

        #region method RemoveCriticalities
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveCriticalities", Name = "RemoveCriticalities")]
        public RemoveGeneralOutModel RemoveCriticalities([FromBody] List<ListInfoWithRowVersion> model)
        {
            var criticality = new BLL.Assets.Criticalities.CriticalitiesGeneral();
            //
            return GeneralCatalogRemove(criticality, model, "RemoveCriticalities");
        }
        #endregion


        #region method GetInfrastructureSegmentById
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetInfrastructureSegmentById", Name = "GetInfrastructureSegmentById")]
        public GeneralGetCatalogByIdOutModel GetInfrastructureSegmentById([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var infrastructureSegment = new BLL.Assets.InfrastructureSegment.InfrastructureSegmentGeneral();
            return GeneralGetCatalogById(infrastructureSegment, model.ID, "GetInfrastructureSegmentById");
        }
        #endregion

        #region method SaveInfrastructureSegment

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveInfrastructureSegment", Name = "SaveInfrastructureSegment")]
        public GeneralCatalogSaveOutModel SaveInfrastructureSegment([FromBody] DTL.Assets.InfrastructureSegment model)
        {

            var infrastructureSegment = new BLL.Assets.InfrastructureSegment.InfrastructureSegmentGeneral();
            //
            return GeneralCatalogSave(infrastructureSegment, model, "SaveInfrastructureSegment");
        }
        #endregion

        #region method RemoveInfrastructureSegments
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveInfrastructureSegment", Name = "RemoveInfrastructureSegment")]
        public RemoveGeneralOutModel InfrastructureSegment([FromBody] List<ListInfoWithRowVersion> model)
        {
            var infrastructureSegment = new BLL.Assets.InfrastructureSegment.InfrastructureSegmentGeneral();
            return GeneralCatalogRemove(infrastructureSegment, model, "RemoveInfrastructureSegment");
        }
        #endregion


        #region method AddWorkFlowScheme
        public sealed class AddWorkFlowSchemeOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddWorkFlowScheme", Name = "AddWorkFlowScheme")]
        public AddWorkFlowSchemeOutModel AddWorkflowScheme([FromBody] InfraManager.WE.DTL.WorkflowScheme model)
        {
            var user = base.CurrentUser;
            if (model == null)
            {
                return new AddWorkFlowSchemeOutModel() { Result = RequestResponceType.BadParamsError };
            }
            try
            {
                Logger.Trace($"AssetApiController.AddWorkFlowScheme userID={user.Id}, userName={user.UserName}");
                Web.BLL.Workflow.WorkflowSchemeHelper.AddWorkflowScheme(model);
                return new AddWorkFlowSchemeOutModel() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new AddWorkFlowSchemeOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method UpdateWorkFlowScheme
        public sealed class UpdateWorkFlowSchemeOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/UpdateWorkFlowScheme", Name = "UpdateWorkFlowScheme")]
        public UpdateWorkFlowSchemeOutModel UpdateWorkFlowScheme([FromBody] InfraManager.WE.DTL.WorkflowScheme model)
        {
            var user = base.CurrentUser;
            if (model == null)
            {
                return new UpdateWorkFlowSchemeOutModel() { Result = RequestResponceType.BadParamsError };
            }
            try
            {
                Logger.Trace($"AssetApiController.UpdateWorkFlowScheme userID={user.Id}, userName={user.UserName}");
                Web.BLL.Workflow.WorkflowSchemeHelper.AddWorkflowScheme(model);
                return new UpdateWorkFlowSchemeOutModel() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new UpdateWorkFlowSchemeOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetTypesWorkflowObjects
        public sealed class GetTypesWorkflowSchemeObjectsOutModel
        {
            public RequestResponceType Result { get; set; }
            public List<TypesWorkflowObjects> WorkFlowObjects { get; set; }
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetTypesWorkflowSchemeObjects", Name = "GetTypesWorkflowSchemeObjects")]
        public GetTypesWorkflowSchemeObjectsOutModel GetTypesWorkflowSchemeObjects()
        {
            try
            {
                return new GetTypesWorkflowSchemeObjectsOutModel() { Result = RequestResponceType.Success, WorkFlowObjects = Web.BLL.Workflow.WorkflowSchemeHelper.GetTypesWorkflowObjects() };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetTypesWorkflowSchemeObjectsOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method GetTypesFormBuilderOutModel
        public sealed class GetTypesFormBuilderOutModel
        {
            public RequestResponceType Result { get; set; }
            public List<TypesWorkflowObjects> WorkFlowObjects { get; set; }
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetTypesFormBuilderObjects", Name = "GetTypesFormBuilderObjects")]
        public GetTypesFormBuilderOutModel GetTypesFormBuilderObjects()
        {
            try
            {
                return new GetTypesFormBuilderOutModel() { Result = RequestResponceType.Success, WorkFlowObjects = Web.BLL.Workflow.WorkflowSchemeHelper.GetTypesFormBuilder() };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new GetTypesFormBuilderOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetWorkflowSchemeByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetWorkflowSchemeByID", Name = "GetWorkflowSchemeByID")]
        public GeneralGetCatalogByIdOutModel GetWorkflowSchemeByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var workflowScheme = new BLL.Workflow.WorkflowScheme.WorkflowSchemeGeneral();
            return GeneralGetCatalogById(workflowScheme, model.ID, "GetWorkflowSchemeByID");
        }
        #endregion


        #region method GetFileSystemByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetFileSystemByID", Name = "GetFileSystemByID")]
        public GeneralGetCatalogByIdOutModel GetFileSystemByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var fileSystem = new BLL.Assets.FileSystem.FileSystemGeneral();
            return GeneralGetCatalogById(fileSystem, model.ID, "GetFileSystemByID");
        }
        #endregion

        #region method SaveFileSystem

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveFileSystem", Name = "SaveFileSystem")]
        public GeneralCatalogSaveOutModel SaveFileSystem([FromBody] DTL.Assets.FileSystem model)
        {
            var fileSystem = new BLL.Assets.FileSystem.FileSystemGeneral();
            //
            return GeneralCatalogSave(fileSystem, model, "SaveFileSystem");
        }
        #endregion

        #region method RemoveFileSystem
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveFileSystem", Name = "RemoveFileSystem")]
        public RemoveGeneralOutModel RemoveFileSystem([FromBody] List<ListInfoWithRowVersion> model)
        {
            var fileSystem = new BLL.Assets.FileSystem.FileSystemGeneral();
            //
            return GeneralCatalogRemove(fileSystem, model, "RemoveFileSystem");
        }
        #endregion


        #region method GetCostCategoryByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCostCategoryByID", Name = "GetCostCategoryByID")]
        public GeneralGetCatalogByIdOutModel GetCostCategoryByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var costCategory = new BLL.Costs.CostCategory.CostCategoryGeneral();
            return GeneralGetCatalogById(costCategory, model.ID, "GetCostCategoryByID");
        }
        #endregion

        #region method SaveCostCategory

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveCostCategory", Name = "SaveCostCategory")]
        public GeneralCatalogSaveOutModel SaveCostCategory([FromBody] DTL.Costs.CostCategory model)
        {
            var costCategory = new BLL.Costs.CostCategory.CostCategoryGeneral();
            //
            return GeneralCatalogSave(costCategory, model, "SaveCostCategory");
        }
        #endregion

        #region method RemoveCostCategory
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveCostCategory", Name = "RemoveCostCategory")]
        public RemoveGeneralOutModel RemoveCostCategory([FromBody] List<ListInfoWithRowVersion> model)
        {
            var costCategory = new BLL.Costs.CostCategory.CostCategoryGeneral();
            //
            return GeneralCatalogRemove(costCategory, model, "RemoveCostCategory");
        }
        #endregion


        #region method GetRFCCategoryByID
        [HttpPost]
        [AcceptVerbs("POSt")]
        [Route("assetApi/GetRFCCategoryByID", Name = "GetRFCCategoryByID")]
        public GeneralGetCatalogByIdOutModel GetRFCCategoryByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var RFCCategory = new BLL.Catalogs.RFCCategory.RFCCategoryGeneral();
            return GeneralGetCatalogById(RFCCategory, model.ID, "GetRFCCategoryByID");
        }
        #endregion

        #region method SaveRFCCategory

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveRFCCategory", Name = "SaveRFCCategory")]
        public GeneralCatalogSaveOutModel SaveRFCCategory([FromBody] DTL.Catalogs.RFCCategory model)
        {
            var RFCCategory = new BLL.Catalogs.RFCCategory.RFCCategoryGeneral();
            //
            return GeneralCatalogSave(RFCCategory, model, "SaveCostCategory");
        }
        #endregion

        #region method RemoveCostCategory
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveRFCCategory", Name = "RemoveRFCCategory")]
        public RemoveGeneralOutModel RemoveRFCCategory([FromBody] List<ListInfoWithRowVersion> model)
        {
            var RFCCategory = new BLL.Catalogs.RFCCategory.RFCCategoryGeneral();
            //
            return GeneralCatalogRemove(RFCCategory, model, "RemoveCostCategory");
        }
        #endregion


        #region method GetCalendarWeekendByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarWeekendByID", Name = "GetCalendarWeekendByID")]
        public GeneralGetCatalogByIdOutModel GetCalendarWeekendByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var calendarWeekend = new BLL.Calendar.CalendarWeekend.CalendarWeekendGeneral();
            return GeneralGetCatalogById(calendarWeekend, model.ID, "GetCalendarWeekendByID");
        }
        #endregion

        #region method SaveCalendarWeekend

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveCalendarWeekend", Name = "SaveCalendarWeekend")]
        public GeneralCatalogSaveOutModel SaveCalendarWeekend([FromBody] DTL.Calendar.CalendarWeekend model)
        {
            var calendarWeekend = new BLL.Calendar.CalendarWeekend.CalendarWeekendGeneral();
            return GeneralCatalogSave(calendarWeekend, model, "SaveCalendarWeekend");
        }
        #endregion

        #region method RemoveCalendarWeekend
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveCalendarWeekend", Name = "RemoveCalendarWeekend")]
        public RemoveGeneralOutModel RemoveCalendarWeekend([FromBody] List<ListInfoWithRowVersion> model)
        {
            var calendarWeekend = new BLL.Calendar.CalendarWeekend.CalendarWeekendGeneral();
            return GeneralCatalogRemove(calendarWeekend, model, "RemoveCalendarWeekend");
        }
        #endregion


        #region method GetCalendarHolidayByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarHolidayByID", Name = "GetCalendarHolidayByID")]
        public GeneralGetCatalogByIdOutModel GetCalendarHolidayByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var calendarHoliday = new BLL.Calendar.CalendarHoliday.CalendarHolidayGeneral();
            return GeneralGetCatalogById(calendarHoliday, model.ID, "GetCalendarHolidayByID");
        }
        #endregion

        #region method SaveCalendarHoliday
        public sealed class SaveCalendarHolidayInModel
        {
            public DTL.Calendar.CalendarHoliday CalendarHoliday { get; set; }
            public List<DTL.Calendar.CalendarHolidayItem> HolidayItems { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveCalendarHoliday", Name = "SaveCalendarHoliday")]
        public GeneralCatalogSaveOutModel SaveCalendarHoliday([FromBody] SaveCalendarHolidayInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace($"AssetApiController.SaveCalendarHoliday userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (BLL.Calendar.CalendarHoliday.ExistsByName(model.CalendarHoliday, dataSource))
                    {
                        return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ExistsByName };
                    }
                    var calendarID = BLL.Calendar.CalendarHoliday.Save(model.CalendarHoliday, user.User.ID, dataSource);

                    var inUseList = BLL.Calendar.CalendarHoliday.SaveItem(model.HolidayItems, dataSource, calendarID);

                    return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, $"ObjectConcurrencyException при сохранении CalendarHoliday. holidayID = {model.CalendarHoliday.ID}");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при сохранении CalendarHoliday. holidayID = {model.CalendarHoliday.ID}");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method RemoveCalendarHoliday
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveCalendarHoliday", Name = "RemoveCalendarHoliday")]
        public RemoveGeneralOutModel RemoveCalendarHoliday([FromBody] List<ListInfoWithRowVersion> model)
        {
            var calendarHolidayItem = new BLL.Calendar.CalendarHoliday.CalendarHolidayGeneral();
            return GeneralCatalogRemove(calendarHolidayItem, model, "RemoveCalendarHoliday");
        }
        #endregion

        #region method GetCalendarHolidayItemByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarHolidayItemByID", Name = "GetCalendarHolidayItemByID")]
        public GeneralGetCatalogByIdOutModel GetCalendarHolidayItemByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var calendarHolidayItem = new BLL.Calendar.CalendarHoliday.CalendarHolidayItemGeneral();
            return GeneralGetCatalogById(calendarHolidayItem, model.ID, "GetCalendarHolidayItemByID");
        }
        #endregion

        #region method GetCalendarHolidayItemsByHolidayID
        public sealed class GetCalendarHolidayItemsByHolidayIDOutModel
        {
            public List<DTL.Calendar.CalendarHolidayItem> HolidayItems { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class GetCalendarHolidayItemsByHolidayIDInModel
        {
            public Guid HolidayID { get; set; }
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarHolidayItemsByHolidayID", Name = "GetCalendarHolidayItemsByHolidayID")]
        public GetCalendarHolidayItemsByHolidayIDOutModel GetCalendarHolidayItemsByHolidayID([FromBody] GetCalendarHolidayItemsByHolidayIDInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GetCalendarHolidayItemsByHolidayIDOutModel() { Result = (RequestResponceType.NullParamsError), HolidayItems = null };
            //
            Logger.Trace($"AssetApiController.GetCalendarHolidayItemsByHolidayID userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var items = BLL.Calendar.CalendarHoliday.GetCalendarHolidayItemsByID(model.HolidayID, dataSource);
                    return new GetCalendarHolidayItemsByHolidayIDOutModel() { Result = RequestResponceType.Success, HolidayItems = items };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении CalendarHolidayItems.");
                return new GetCalendarHolidayItemsByHolidayIDOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }
        #endregion

        #region method SaveCalendarHolidayItem

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveCalendarHolidayItem", Name = "SaveCalendarHolidayItem")]
        public GeneralCatalogSaveOutModel SaveCalendarHoliday([FromBody] DTL.Calendar.CalendarHolidayItem model)
        {
            var calendarHolidayItem = new BLL.Calendar.CalendarHoliday.CalendarHolidayItemGeneral();
            return GeneralCatalogSave(calendarHolidayItem, model, "SaveCalendarHolidayItem");
        }
        #endregion

        #region method RemoveCalendarHolidayitem
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveCalendarHolidayitem", Name = "RemoveCalendarHolidayitem")]
        public RemoveGeneralOutModel RemoveCalendarHolidayitem([FromBody] List<ListInfoWithRowVersion> model)
        {
            var calendarHolidayItem = new BLL.Calendar.CalendarHoliday.CalendarHolidayItemGeneral();
            return GeneralCatalogRemove(calendarHolidayItem, model, "RemoveCalendarHolidayitem");
        }
        #endregion


        #region method GetCommercialModelTypeList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetCommercialModelTypeList", Name = "GetCommercialModelTypeList")]
        public List<SoftwareModelType> GetCommercialModelTypeList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return null;
                //
                Logger.Trace("AssetApiController.GetCommercialModelTypeList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("AssetApiController.GetCommercialModelTypeList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareModel.GetTypeList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка типов ПО.");
                return null;
            }
        }
        #endregion

        #region method GetUsingModelList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetUsingModelList", Name = "GetUsingModelList")]
        public List<SoftwareUsingModel> GetUsingModelList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return null;
                //
                Logger.Trace("AssetApiController.GetUsingModelList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("AssetApiController.GetUsingModelList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareModel.GetUsingModelList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка использования модели ПО.");
                return null;
            }
        }
        #endregion

        #region method GetLicenseControlList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLicenseControlList", Name = "GetLicenseControlList")]
        public List<DTL.SimpleEnum> GetLicenseControlList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return null;
                //
                Logger.Trace("assetApi.GetLicenseControlList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("assetApi.GetLicenseControlList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareModel.GetLicenseControl();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка лицезирования.");
                return null;
            }
        }
        #endregion

        #region method GetLicenseLanguageList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetLicenseLanguageList", Name = "GetLicenseLanguageList")]
        public List<DTL.SimpleEnum> GetLicenseLanguageList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return null;
                //
                Logger.Trace("assetApi.GetLicenseLanguageList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("assetApi.GetLicenseLanguageList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareModel.GetLicenseLanguage();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка языков.");
                return null;
            }
        }
        #endregion

        #region method GetSoftwareModelTemplateList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSoftwareModelTemplateList", Name = "GetSoftwareModelTemplateList")]
        public List<DTL.SimpleEnum> GetSoftwareModelTemplateList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return null;
                //
                Logger.Trace("assetApi.GetSoftwareModelTemplateList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("assetApi.GetSoftwareModelTemplateList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = SoftwareModel.GetSoftwareModelTemplate();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка шаблонов.");
                return null;
            }
        }
        #endregion

        #region method AddSoftwareModelOutModel
        public sealed class AddSoftwareModelOutModel

        {
            public RequestResponceType Result { get; set; }
            public string Message { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddSoftwareModel", Name = "AddSoftwareModel")]
        public AddSoftwareModelOutModel AddSoftwareModel([FromBody] SoftwareModelRegister model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new AddSoftwareModelOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.AddSoftwareModel userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                SoftwareModelHelper.Save(model);
                return new AddSoftwareModelOutModel() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddSoftwareModelObject");
                return new AddSoftwareModelOutModel() { Result = RequestResponceType.GlobalError , Message = ex.Message};
            }
        }
        #endregion

        #region method RemoveSoftwareModel
        public sealed class RemoveSoftwareModelInModel

        {
            public Guid ID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveSoftwareModel", Name = "RemoveSoftwareModel")]
        public RequestResponceType RemoveSoftwareModel([FromBody] RemoveSoftwareModelInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return RequestResponceType.NullParamsError;
            //
            Logger.Trace("AssetApiController.RemoveSoftwareModel userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_DELETE_SOFTWAREMODEL))
                {
                    Logger.Trace("AssetApiController.RemoveSoftwareModel userID={0}, userName={1}", user.Id, user.UserName);
                    return RequestResponceType.OperationError;
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    SoftwareModelHelper.RemoveSoftwareModel(model.ID);
                    return RequestResponceType.Success;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RemoveSoftwareModel");
                return RequestResponceType.GlobalError;
            }
        }
        #endregion

        #region GetSoftwareModel
        public sealed class SoftwareModelOutModel
        {
            public SoftwareModel SoftwareModel { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class SoftwareModelInModel
        {
            public Guid ID { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSoftwareModel", Name = "GetSoftwareModel")]
        public SoftwareModelOutModel GetSoftwareModel([FromQuery] SoftwareModelInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new SoftwareModelOutModel() { SoftwareModel = null, Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.GetSoftwareModelOutModel userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_SOFTWAREMODEL))
                {
                    Logger.Trace("AssetApiController.GetSoftwareModel userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return new SoftwareModelOutModel() { SoftwareModel = null, Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = SoftwareModel.Get(model.ID, dataSource);
                    return new SoftwareModelOutModel() { SoftwareModel = retval, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSoftwareModel is NULL, id: '{0}'", model.ID);
                return new SoftwareModelOutModel() { SoftwareModel = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSoftwareModel, id: '{0}'", model.ID);
                return new SoftwareModelOutModel() { SoftwareModel = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetSoftwareModelViewFilters
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSoftwareModelViewFilters", Name = "GetSoftwareModelViewFilters")]
        public List<SoftwareModelViewFilter> GetSoftwareModelViewFilters()
        {
            //
            Logger.Trace("AssetApiController.GetSoftwareModelViewFilters");
            using (var dataSource = DataSource.GetDataSource())
            {
                var retval = SoftwareModelViewFilter.GetFilterList(dataSource);
                return retval;
            }
        }
        #endregion

        #region method SetSoftwareModelViewFilter
        public sealed class SetSoftwareModelViewFilterOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        public sealed class SetSoftwareModelViewFilterInModel
        {
            public Guid FilterID { get; set; }
        }
        [HttpGet]
        [AcceptVerbs("POST")]
        [Route("assetApi/SetSoftwareModelViewFilter", Name = "SetSoftwareModelViewFilter")]
        public SetSoftwareModelViewFilterOutModel SetSoftwareModelViewFilter([FromBody] SetSoftwareModelViewFilterInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new SetSoftwareModelViewFilterOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("AssetApiController.SetSoftwareModelViewFilter");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    SoftwareModelViewFilter.SetFilter(model.FilterID, user.User.ID, dataSource);
                    return new SetSoftwareModelViewFilterOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "SetSoftwareModelViewFilter is NULL");
                return new SetSoftwareModelViewFilterOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "SetSoftwareModelViewFilter");
                return new SetSoftwareModelViewFilterOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        public sealed class GeneralGetCatalogByIdOutModelInt
        {
            public DTL.Assets.CatalogGeneralInt Catalog { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public GeneralGetCatalogByIdOutModelInt GeneralGetCatalogById(ICatalogGeneralInt catalogGeneral, int id, string friendlyCatalogNameForTrace)
        {
            var user = base.CurrentUser;
            if (user == null || id.Equals(null))
                return new GeneralGetCatalogByIdOutModelInt() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace($"AssetApiController.{friendlyCatalogNameForTrace} userID={user.Id}, userName={user.UserName}, ID={id}");
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    DTL.Assets.CatalogGeneralInt catalog = catalogGeneral.Get(id, user.User.ID, dataSource);
                    //
                    return new GeneralGetCatalogByIdOutModelInt() { Result = (RequestResponceType.Success), Catalog = catalog };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, $"Ошибка при получении справочника {friendlyCatalogNameForTrace}.");
                return new GeneralGetCatalogByIdOutModelInt() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при получении справочника {friendlyCatalogNameForTrace}.");
                return new GeneralGetCatalogByIdOutModelInt() { Result = (RequestResponceType.GlobalError) };
            }
        }

        public GeneralCatalogSaveOutModel GeneralCatalogSave(ICatalogGeneralInt catalog, DTL.Assets.CatalogGeneralInt model, string friendlyCatalogNameForTrace)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace($"AssetApiController.{friendlyCatalogNameForTrace} userID={user.Id}, userName={user.UserName}, ID={model.ID}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (catalog.ExistsByName(model, dataSource))
                    {
                        Logger.Trace($"Тип {friendlyCatalogNameForTrace} уже существует, имя {model.ID}");
                        return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.ExistsByName) };
                    }
                    catalog.Save(model, user.User.ID, dataSource);
                    //
                    return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.Success) };
                }
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, $"Ошибка при редактировании справочника {friendlyCatalogNameForTrace}.");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, $"Ошибка при редактировании справочника {friendlyCatalogNameForTrace}.");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при сохраннении справочника {friendlyCatalogNameForTrace}.");
                return new GeneralCatalogSaveOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }

        private RemoveGeneralOutModel GeneralCatalogRemove(ICatalogGeneralInt catalog, List<SimpleEnum> model, string friendlyCatalogNameForTrace)
        {
            var user = base.CurrentUser;
            if (model == null || model.Count < 1)
                return new RemoveGeneralOutModel() { Result = (RequestResponceType.NullParamsError) };
            //
            Logger.Trace($"AssetApiController.{friendlyCatalogNameForTrace} userID={0}, userName={1}",
            user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    List<string> InUseList = new List<string>();
                    List<string> ConcurrencyList = new List<string>();
                    foreach (var el in model)
                    {
                        try
                        {
                            catalog.Remove(el, dataSource);
                        }
                        catch (SqlException e)
                        {
                            if (e.Number == 547) // Foreign Key violation
                                InUseList.Add(el.Name);
                        }
                        catch (ObjectConcurrencyException)
                        {
                            ConcurrencyList.Add(el.Name);
                        }
                    }
                    //
                    return new RemoveGeneralOutModel() { Result = (RequestResponceType.Success), InUseList = InUseList, ConcurrencyList = ConcurrencyList };
                }
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, $"Ошибка при удалении справочника {friendlyCatalogNameForTrace}.");
                return new RemoveGeneralOutModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, $"Ошибка при удалении справочника {friendlyCatalogNameForTrace}.");
                return new RemoveGeneralOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при удалении справочника {friendlyCatalogNameForTrace}");
                return new RemoveGeneralOutModel() { Result = (RequestResponceType.GlobalError) };
            }
        }

        #region methods GetSlotTypes
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSlotTypeList", Name = "GetSlotTypeList")]
        public DTL.SimpleEnum[] GetSlotTypesList()
        {
            var user = base.CurrentUser;
            Logger.Trace($"assetApi.GetSlotTypeList userID={user.Id}, userName={user.UserName}");

            List<DTL.Assets.SlotType> slotTypes = new List<DTL.Assets.SlotType>();// BLL.Web.Assets.SlotType.GetList(DataSource.GetDataSource());
            DTL.SimpleEnum[] result = new DTL.SimpleEnum[slotTypes.Count];

            for (int i = 0; i < slotTypes.Count; i++)
            {
                result[i] = new DTL.SimpleEnum()
                {
                    ID = slotTypes[i].ID,
                    Name = slotTypes[i].Name
                };
            }

            return result;
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSlotType", Name = "GetSlotType")]
        public GeneralGetCatalogByIdOutModelInt GetSlotType([FromBody] DTL.Assets.SlotType model)
        {
            var slot = new BLL.Assets.SlotType.SlotTypeGeneral();
            return GeneralGetCatalogById(slot, model.ID, "GetSlotType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveSlotType", Name = "SaveSlotType")]
        public GeneralCatalogSaveOutModel UpdateSlotType([FromBody] DTL.Assets.SlotType model)
        {
            var slot = new BLL.Assets.SlotType.SlotTypeGeneral();
            return GeneralCatalogSave(slot, model, "SaveSlotType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveSlotType", Name = "RemoveSlotType")]
        public RemoveGeneralOutModel RemoveSlotType([FromBody] List<SimpleEnum> model)
        {
            var slot = new BLL.Assets.SlotType.SlotTypeGeneral();
            return GeneralCatalogRemove(slot, model, "RemoveSlotType");
        }
        #endregion


        #region methods TelephoneType

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetTelephoneType", Name = "GetTelephoneType")]
        public GeneralGetCatalogByIdOutModel GetTelephoneType([FromBody] DTL.Assets.TelephoneType model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GeneralGetCatalogByIdOutModel() { Catalog =null, Result = RequestResponceType.NullParamsError };

            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_TELEPHONETYPE))
            {
                Logger.Trace("AssetApiController.GetTelephoneType userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                return new GeneralGetCatalogByIdOutModel() { Catalog = null, Result = RequestResponceType.OperationError };
            }


            var telephone = new TelephoneType.TelephoneTypeGeneral();
            return GeneralGetCatalogById(telephone, model.ID, "GetTelephoneType");
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveTelephoneType", Name = "RemoveTelephoneType")]
        public RemoveGeneralOutModel RemoveTelephoneType([FromBody] List<ListInfoWithRowVersion> models)
        {
            var telephone = new TelephoneType.TelephoneTypeGeneral();
            return GeneralCatalogRemove(telephone, models, "RemoveTelephoneType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveTelephoneType", Name = "SaveTelephoneType")]
        public GeneralCatalogSaveOutModel SaveTelephoneType([FromBody] DTL.Assets.TelephoneType model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.NullParamsError };

            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_TELEPHONETYPE) ||
                !user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_TELEPHONETYPE))
            {
                Logger.Trace("AssetApiController.SaveTelephoneType userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.OperationError };
            }

            var telephone = new BLL.Assets.TelephoneType.TelephoneTypeGeneral();
            return GeneralCatalogSave(telephone, model, "SaveTelephoneType");
        }
        #endregion


        #region methods TechnologyType
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveTechnologyType", Name = "RemoveTechnologyType")]
        public RemoveGeneralOutModel RemoveTechnologyType([FromBody] List<SimpleEnum> models)
        {
            var technology = new BLL.Assets.TechnologyType.TechnologyTypeGeneral();
            return GeneralCatalogRemove(technology, models, "RemoveTechnologyType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveTechnologyType", Name = "SaveTechnologyType")]
        public GeneralCatalogSaveOutModel SaveTechnologyType([FromBody] DTL.Assets.TechnologyType model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.NullParamsError };

            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_TECHNOLOGYTYPE) || 
                !user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_TECHNOLOGYTYPE))
            {
                Logger.Trace("AssetApiController.SaveTechnologyType userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.OperationError };
            }

            var technology = new BLL.Assets.TechnologyType.TechnologyTypeGeneral();
            return GeneralCatalogSave(technology, model, "SaveTechnologyType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetTechnologyType", Name = "GetTechnologyType")]
        public GeneralGetCatalogByIdOutModelInt GetTechnologyType([FromBody] DTL.Assets.TechnologyType model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GeneralGetCatalogByIdOutModelInt() { Catalog = null, Result = RequestResponceType.NullParamsError };

            if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_TECHNOLOGYTYPE))
            {
                Logger.Trace("AssetApiController.GetTechnologyType userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                return new GeneralGetCatalogByIdOutModelInt() { Catalog = null, Result = RequestResponceType.OperationError };
            }

            var technology = new BLL.Assets.TechnologyType.TechnologyTypeGeneral();
            var result = GeneralGetCatalogById(technology, model.ID, "GetTechnologyType");
            return result;
        }
        #endregion


        #region methods ConnectorType

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetConnectorType", Name = "GetConnectorType")]
        public GeneralGetCatalogByIdOutModelInt GetConnectorType([FromBody] DTL.Assets.ConnectorType model)
        {
            var connector = new BLL.Assets.ConnectorType.ConnectorTypeGeneral();
            return GeneralGetCatalogById(connector, model.ID, "GetConnectorType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveConnectorType", Name = "SaveConnectorType")]
        public GeneralCatalogSaveOutModel SaveConnectorType([FromBody] DTL.Assets.ConnectorType model)
        {
            var connector = new BLL.Assets.ConnectorType.ConnectorTypeGeneral();
            return GeneralCatalogSave(connector, model, "SaveConnectorType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveConnectortype", Name = "RemoveConnectortype")]
        public RemoveGeneralOutModel RemoveConnectortype([FromBody] List<SimpleEnum> models)
        {
            var connector = new BLL.Assets.ConnectorType.ConnectorTypeGeneral();
            return GeneralCatalogRemove(connector, models, "RemoveConnectortype");
        }

        #endregion


        #region methods CartridgeType
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCartridgeType", Name = "GetCartridgeType")]
        public GeneralGetCatalogByIdOutModel GetCartridgeType([FromBody] DTL.Assets.CartridgeType model)
        {
            var cartridge = new BLL.Assets.CartridgeType.CartridgeTypeGeneral();
            return GeneralGetCatalogById(cartridge, model.ID, "GetCartridgeTypeById");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveCartridgeType", Name = "SaveCartridgeType")]
        public GeneralCatalogSaveOutModel SaveCartridgeType([FromBody] DTL.Assets.CartridgeType model)
        {
            var cartridge = new BLL.Assets.CartridgeType.CartridgeTypeGeneral();
            return GeneralCatalogSave(cartridge, model, "SaveCartridgeType");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveCartridgeType", Name = "RemoveCartridgeType")]
        public RemoveGeneralOutModel RemoveCartridgeType([FromBody] List<ListInfoWithRowVersion> models)
        {
            var cartridge = new BLL.Assets.CartridgeType.CartridgeTypeGeneral();
            return GeneralCatalogRemove(cartridge, models, "RemoveCartridgeType");
        }
        #endregion


        #region method FormBuilderGetTypes
        public sealed class FormBuilderGetTypesOutModel
        {
            public List<BLL.FormBuilder.FormTabField> ElementsTypes { get; set; }
            public List<BLL.FormBuilder.FormTabField> ElementsStructure { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/FormBuilderGetTypes", Name = "FormBuilderGetTypes")]
        public FormBuilderGetTypesOutModel FormBuilderGetTypes()
        {
            List<WorkflowActivityFormField> ElementsTypes = BLL.FormBuilder.Form.GetElementsTypes();
            List<WorkflowActivityFormField> ElementsStructure = BLL.FormBuilder.Form.GetElementsStructure();
            return new FormBuilderGetTypesOutModel()
            {
                ElementsTypes = BLL.FormBuilder.FormTabField.Get(ElementsTypes),
                ElementsStructure = BLL.FormBuilder.FormTabField.Get(ElementsStructure)
            };
        }
        #endregion

        #region method FormBuilderGetForm
        public sealed class FormBuidlerFormGetModelOut
        {
            public BLL.FormBuilder.FormForTable Form { get; set; }
            public List<Elements> Elements { get; set; }
            public RequestResponceType Result { get; set; }
        }

        public sealed class Elements
        {
            public WorkflowActivityFormTab Tab { get; set; }
            public List<BLL.FormBuilder.FormTabField> TabElements { get; set; }
        }

        public sealed class FormBuilderFormGetModelIn
        {
            public Guid formID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/FormBuilderGetForm", Name = "FormBuilderGetForm")]
        public FormBuidlerFormGetModelOut FormBuilderGetForm([FromBody] FormBuilderFormGetModelIn model)
        {
            FormBuidlerFormGetModelOut result = new FormBuidlerFormGetModelOut() { Result = RequestResponceType.Success };
            result.Elements = new List<Elements>();
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var fullForm = Form.GetFullForm(model.formID, dataSource);

                    var form = new FormForTable(fullForm.Form);
                    if (form == null)
                    {
                        return new FormBuidlerFormGetModelOut { Result = RequestResponceType.ObjectDeleted };
                    }
                    result.Form = form;
                    foreach (var tab in fullForm.Elements)
                    {
                        var element = new Elements() { Tab = tab.Tab, TabElements = BLL.FormBuilder.FormTabField.Get(tab.TabElements) };
                        result.Elements.Add(element);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при получении формы FormBuilder.");
                return new FormBuidlerFormGetModelOut() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method FormBuidlerDeleteForm
        public sealed class FormBuilderDeleteFormModelIn
        {
            public Guid ID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/FormBuilderDeleteForm", Name = "FormBuilderDeleteForm")]
        public RemoveGeneralOutModel FormBuilderDeleteForm([FromBody] FormBuilderDeleteFormModelIn model)
        {
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var form = Form.GetForm(model.ID, dataSource); //TODO
                    if (form.Status == FormBuilderFormStatus.Published)
                    {
                        return new RemoveGeneralOutModel() { Result = RequestResponceType.BadParamsError };
                    }

                    BLL.FormBuilder.Form.RemoveForm(model.ID, dataSource);
                }
                return new RemoveGeneralOutModel() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при удалении формы у формбилдера.");
                return new RemoveGeneralOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method FormBuilderSaveForm
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/FormBuilderSaveForm", Name = "FormBuilderFormSave")]
        public GeneralCatalogSaveOutModel FormBuilderFormSave([FromBody] FormBuilderForm model)
        {
            var user = base.CurrentUser;
            Logger.Trace($"AssetApiController.FormBuilderSaveForm userID={user.User.ID}");
            if (model == null)
            {
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.NullParamsError };
            }
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    Form.SaveFullForm(model, dataSource);
                }
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при сохранении формы у формбилдера.");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region FormBuilderSavePropertiesForm

        public sealed class FormBuilderCreateFormModelIn
        {
            public Guid Data { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/FormBuilderSavePropertiesForm", Name = "FormBuilderSavePropertiesForm")]
        public FormBuilderCreateFormModelIn FormBuilderCreateForm([FromBody] WorkflowActivityForm model)
        {
            var user = base.CurrentUser;
            Logger.Trace($"AssetApiController.FormBuilderSavePropertiesForm userID={user.User.ID}");
            if (model == null)
            {
                return new FormBuilderCreateFormModelIn() { Result = RequestResponceType.NullParamsError };
            }
            try
            {
                Guid ID = Guid.Empty;
                using (var dataSource = DataSource.GetDataSource())
                {
                    ID = BLL.FormBuilder.Form.SaveForm(model, dataSource);
                }
                return new FormBuilderCreateFormModelIn() { Data = ID, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                return new FormBuilderCreateFormModelIn() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method FormBuilderPublishForm
        public sealed class FormBuilderPublishFormInModel
        {
            public Guid FormID { get; set; }
        }
        public sealed class FormBuilderPublishFormOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/FormBuilderPublishForm", Name = "FormBuilderPublishForm")]
        public FormBuilderPublishFormOutModel FormBuilderPublishForm([FromBody] FormBuilderGetPropertiesFormInModel model)
        {
            var user = base.CurrentUser;
            Logger.Trace($"AssetApiController.FormBuilderPublishForm userID={user.User.ID}");
            if (model == null)
            {
                return new FormBuilderPublishFormOutModel() { Result = RequestResponceType.NullParamsError };
            }
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    BLL.FormBuilder.Form.PublishForm(model.FormID, dataSource);
                }
                return new FormBuilderPublishFormOutModel() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                return new FormBuilderPublishFormOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region FormBuilderGetPropertiesForm
        public sealed class FormBuilderGetPropertiesFormInModel
        {
            public Guid FormID { get; set; }
        }

        public sealed class FormBuilderGetPropertiesFormOutModel
        {
            public FormForTable Data { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/FormBuilderGetPropertiesForm", Name = "FormBuilderGetPropertiesForm")]
        public FormBuilderGetPropertiesFormOutModel FormBuilderGetPropertiesForm([FromBody] FormBuilderGetPropertiesFormInModel model)
        {
            var user = base.CurrentUser;
            Logger.Trace($"AssetApiController.FormBuilderGetPropertiesForm userID={user.User.ID}");
            if (model == null)
            {
                return new FormBuilderGetPropertiesFormOutModel() { Result = RequestResponceType.NullParamsError };
            }
            try
            {
                FormForTable result = null;
                using (var dataSource = DataSource.GetDataSource())
                {
                    result = BLL.FormBuilder.Form.GetFormForTable(model.FormID, dataSource);
                }
                return new FormBuilderGetPropertiesFormOutModel() { Data = result, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                return new FormBuilderGetPropertiesFormOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method CloneFormBuilderForm
        public sealed class CloneFormBuilderFormModelIn
        {
            public Guid FormID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Identifier { get; set; }
        }

        public sealed class CloneFormBuilderFormModelOut
        {
            public Guid Data { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/CloneAndSaveFormBuilderForm", Name = "CloneAndSaveFormBuilderForm")]
        public CloneFormBuilderFormModelOut CloneFormBuilderForm([FromBody] CloneFormBuilderFormModelIn model)
        {
            var user = base.CurrentUser;
            Logger.Trace($"AssetApiController.CloneAndSaveFormBuilderForm userID={user.User.ID}");
            if (model == null)
            {
                return new CloneFormBuilderFormModelOut() { Result = RequestResponceType.NullParamsError };
            }
            try
            {
                Guid formID = Guid.Empty;
                using (var dataSource = DataSource.GetDataSource())
                {
                    formID = BLL.FormBuilder.Form.CloneAndSaveFullForm(model.FormID, model.Name, model.Description, model.Identifier, dataSource);
                }
                return new CloneFormBuilderFormModelOut() { Data = formID, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                return new CloneFormBuilderFormModelOut() { Result = RequestResponceType.GlobalError };
            }
        }

        #endregion

        #region method RemoveParameterEnum
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveParameterEnum", Name = "RemoveParameterEnum")]
        public RemoveGeneralOutModel RemoveParameterEnum([FromBody] List<ListInfoWithRowVersion> model)
        {
            var parameterEnum = new BLL.Parameters.ParameterEnum.ParameterEnumGeneral();
            return GeneralCatalogRemove(parameterEnum, model, "ParameterEnum");
        }
        #endregion

        #region method SaveParameterEnum
        public sealed class SavePrameterEnumInModel
        {
            public DTL.Parameters.ParameterEnum ParameterEnum { get; set; }
            public List<ParameterEnumValues> ParameterEnumValues { get; set; }
        }


        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveParameterEnum", Name = "SaveParameterEnum")]
        public GeneralCatalogSaveOutModel SaveParameterEnum([FromBody] SavePrameterEnumInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace($"AssetApiController.SaveParameterEnum userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (BLL.Parameters.ParameterEnum.ExistsByName(model.ParameterEnum, dataSource))
                    {
                        return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ExistsByName };
                    }
                    var paremeterID = BLL.Parameters.ParameterEnum.Save(model.ParameterEnum, user.User.ID, dataSource);
                    var existsByName = BLL.Parameters.ParameterEnumValue.Save(model.ParameterEnumValues, paremeterID, dataSource);
                    return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, $"ObjectConcurrencyException при сохранении SaveParameterEnum. SaveParameterEnum = {model.ParameterEnum.ID}");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.ConcurrencyError };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при сохранении SaveParameterEnum. SaveParameterEnum = {model.ParameterEnum.ID}");
                return new GeneralCatalogSaveOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetParameterEnumValue
        public sealed class GetParameterEnumValueInModel
        {
            public Guid ParameterEnumID { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetParameterEnumValue", Name = "GetParameterEnumValue")]
        public GeneralGetModelOut<List<SyncParameterValue>> GetParameterEnumValue([FromBody] GetParameterEnumValueInModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new GeneralGetModelOut<List<SyncParameterValue>>() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace($"AssetApiController.GetParameterEnumValue userID={user.Id}, userName={user.UserName}");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    return new GeneralGetModelOut<List<SyncParameterValue>>() { Result = RequestResponceType.Success, Catalog = BLL.Parameters.ParameterEnumValue.GetParameterEnumValuesWithSubclasses(model.ParameterEnumID, dataSource) };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Ошибка при GetParameterEnumValue. ParameterEnumID = {model.ParameterEnumID}");
                return new GeneralGetModelOut<List<SyncParameterValue>>() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetParameterEnumValueByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetParameterEnumValueByID", Name = "GetParameterEnumValueByID")]
        public GeneralGetCatalogByIdOutModel GetParameterEnumValueByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var parameterEnum = new BLL.Parameters.ParameterEnum.ParameterEnumGeneral();
            return GeneralGetCatalogById(parameterEnum, model.ID, "GetParameterEnumValueByID");
        }
        #endregion


        #region method GetParameterEnumValueByID
        public sealed class PublishWorkflowSchemeModelIn
        {
            public Guid workflowSchemeID { get; set; }
        }

        public sealed class PublishWorkflowSchemeModelOut
        {
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/PublishWorkflowScheme", Name = "PublishWorkflowScheme")]
        public PublishWorkflowSchemeModelOut PublishWorkflowScheme([FromBody] PublishWorkflowSchemeModelIn model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new PublishWorkflowSchemeModelOut() { Result = RequestResponceType.NullParamsError };

            try
            {
                BLL.Workflow.WorkflowSchemeHelper.PublishWorkflowScheme(model.workflowSchemeID);
                return new PublishWorkflowSchemeModelOut() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                return new PublishWorkflowSchemeModelOut() { Result = RequestResponceType.GlobalError };
            }

        }
        #endregion


        #region AddAdmittedQueue
        public sealed class AddAdmittedQueueInModel
        {
            public Guid KBAID { get; set; }
            public Guid QueueID { get; set; }
        }
        public sealed class AddAdmittedQueueOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddAdmittedQueue", Name = "AddAdmittedQueue")]
        public AddAdmittedQueueOutModel AddAdmittedQueue(AddAdmittedQueueInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddAdmittedQueueOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.KBAID);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    KBArticle.AddAdmittedQueue(model.KBAID, model.QueueID, dataSource);
                    return new AddAdmittedQueueOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "AddAdmittedQueueOutModel is NULL, id: '{0}'", model.KBAID);
                return new AddAdmittedQueueOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddAdmittedQueueOutModel, id: '{0}'", model.KBAID);
                return new AddAdmittedQueueOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AddAdmittedUsers
        public sealed class AddAdmittedUsersInModel
        {
            public Guid KBAID { get; set; }
            public Guid UserID { get; set; }
        }
        public sealed class AddAdmittedUsersOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddAdmittedUsers", Name = "AddAdmittedUsers")]
        public AddAdmittedUsersOutModel AddAdmittedUsers(AddAdmittedUsersInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddAdmittedUsersOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.KBAID);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    KBArticle.AddAdmittedUser(model.KBAID, model.UserID, dataSource);
                    return new AddAdmittedUsersOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "AddAdmittedUsersOutModel is NULL, id: '{0}'", model.KBAID);
                return new AddAdmittedUsersOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddAdmittedUsersOutModel, id: '{0}'", model.KBAID);
                return new AddAdmittedUsersOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method SaveProblemTypeNew
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveProblemTypeNew", Name = "SaveProblemTypeNew")]
        public GeneralCatalogSaveOutModel SaveProblemTypeNew([FromBody] DTL.SD.Problems.ProblemTypeNew model)
        {
            var problemTypeNew = new BLL.SD.Problems.ProblemTypeNew.ProblemTypesNewGeneral();
            return GeneralCatalogSave(problemTypeNew, model, "SaveProblemTypeNew");
        }
        #endregion

        #region method RemoveProblemTypeNew
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveProblemTypeNew", Name = "RemoveProblemTypeNew")]
        public RemoveGeneralOutModel RemoveProblemTypeNew([FromBody] List<ListInfoWithRowVersion> model)
        {
            var problemTypeNew = new BLL.SD.Problems.ProblemTypeNew.ProblemTypesNewGeneral();
            return GeneralCatalogRemove(problemTypeNew, model, "RemoveProblemTypeNew");
        }
        #endregion

        #region method GetProblemsByParentID
        public sealed class GetProblemsByParentIDModelIn
        {
            public Guid ProblemParentID { get; set; } = Guid.Parse("00000000-0702-0000-0000-000000000000");
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetProblemsByParentID", Name = "GetProblemsByParentID")]
        public GeneralGetModelOut<List<DTL.SD.Problems.ProblemTypeNew>> GetProblemsByParentID([FromBody] GetProblemsByParentIDModelIn model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GeneralGetModelOut<List<DTL.SD.Problems.ProblemTypeNew>> { Result = RequestResponceType.NullParamsError };

            Logger.Trace("AssetApiController.GetProblemsByParentID");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var problemList = BLL.SD.Problems.ProblemTypeNew.GetProblemsIDByParentID(model.ProblemParentID);
                    return new GeneralGetModelOut<List<DTL.SD.Problems.ProblemTypeNew>> { Catalog = problemList, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetProblemsByParentID");
                return new GeneralGetModelOut<List<DTL.SD.Problems.ProblemTypeNew>> { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetProblemByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetProblemByID", Name = "GetProblemByID")]
        public GeneralGetCatalogByIdOutModel GetProblemByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var problemTypeNew = new BLL.SD.Problems.ProblemTypeNew.ProblemTypesNewGeneral();
            return GeneralGetCatalogById(problemTypeNew, model.ID, "GetProblemByID");
        }
        #endregion


        #region AddAdmittedSubDivision
        public sealed class AddAdmittedSubDivisionInModel
        {
            public Guid KBAID { get; set; }
            public Guid SubDivisionID { get; set; }
            public bool WithSub { get; set; }
        }
        public sealed class AddAdmittedSubDivisionOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddAdmittedSubDivision", Name = "AddAdmittedSubDivision")]
        public AddAdmittedSubDivisionOutModel AddAdmittedSubDivision(AddAdmittedSubDivisionInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddAdmittedSubDivisionOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.KBAID);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    KBArticle.AddAdmittedSubDivision(model.KBAID, model.SubDivisionID, model.WithSub, dataSource);
                    return new AddAdmittedSubDivisionOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "AddAdmittedSubDivisionOutModel is NULL, id: '{0}'", model.KBAID);
                return new AddAdmittedSubDivisionOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddAdmittedQueueOutModel, id: '{0}'", model.KBAID);
                return new AddAdmittedSubDivisionOutModel() { Result = RequestResponceType.GlobalError };
            }
        }


        public sealed class AsseccID
        {
            public Guid KBArticleID { get; set; }
            public Guid ObjectID { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/RemoveAccessObject", Name = "RemoveAccessObject")]
        public ResultWithMessage RemoveAccessObject(AsseccID model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("AccountApiController.RemoveAccessObject userID={0}, userName={1}}", user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    KBArticle.RemoveAccessObject(model.KBArticleID, model.ObjectID, dataSource);
                    //
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError, ex.Message);
            }
        }
        #endregion

        #region AddAdmittedOrganization
        public sealed class AddAdmittedOrganizationInModel
        {
            public Guid KBAID { get; set; }
            public Guid OrganizationID { get; set; }
        }
        public sealed class AddAdmittedOrganizatioOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddAdmittedOrganization", Name = "AddAdmittedOrganization")]
        public AddAdmittedOrganizatioOutModel AddAdmittedOrganization(AddAdmittedOrganizationInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddAdmittedOrganizatioOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetAsset userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.KBAID);
            //
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    KBArticle.AddAdmittedOrganization(model.KBAID, model.OrganizationID, dataSource);
                    return new AddAdmittedOrganizatioOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "AddAdmittedOrganizatioOutModel is NULL, id: '{0}'", model.KBAID);
                return new AddAdmittedOrganizatioOutModel() { Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddAdmittedQueueOutModel, id: '{0}'", model.KBAID);
                return new AddAdmittedOrganizatioOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetSoftwareModelFiltersByID
        public sealed class GetSoftwareModelFiltersByIDOutModel
        {
            public Guid? FilterID { get; set; }
            public string FilterName { get; set; }
            public string FilterСommand { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSoftwareModelFiltersByID", Name = "GetSoftwareModelFiltersByID")]
        public GetSoftwareModelFiltersByIDOutModel GetSoftwareModelFiltersByID()
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSoftwareModelFiltersByIDOutModel() { FilterID = null, FilterName = null, FilterСommand = null, Result = RequestResponceType.NullParamsError };

            Logger.Trace("AssetApiController.GetSoftwareModelFiltersByID");
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var filter = SoftwareModelViewFilter.GetFilterByUser(user.User.ID, dataSource);
                    return new GetSoftwareModelFiltersByIDOutModel() { FilterID = filter.FilterID, FilterName = filter.FilterName, FilterСommand = filter.FilterСommand, Result = RequestResponceType.Success };
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "GetSoftwareModelFiltersByID is NULL");
                return new GetSoftwareModelFiltersByIDOutModel() { FilterID = null, FilterName = null, FilterСommand = null, Result = RequestResponceType.ObjectDeleted };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetSoftwareModelFiltersByID");
                return new GetSoftwareModelFiltersByIDOutModel() { FilterID = null, FilterName = null, FilterСommand = null, Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region method GetExclusionByID
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetExclusionByID", Name = "GetExclusionByID")]
        public GeneralGetCatalogByIdOutModel GetExclusionByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var exclusion = new BLL.Calendar.Exclusion.ExclusionGeneral();
            return GeneralGetCatalogById(exclusion, model.ID, "GetExclusionByID");
        }
        #endregion

        #region method SaveExclusion

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveExclusion", Name = "SaveExclusion")]
        public GeneralCatalogSaveOutModel SaveExclusion([FromBody] DTL.Exclusions.Exclusion model)
        {
            var exclusion = new BLL.Calendar.Exclusion.ExclusionGeneral();
            return GeneralCatalogSave(exclusion, model, "SaveExclusion");
        }
        #endregion

        #region method RemoveExclusion
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveExclusion", Name = "RemoveExclusion")]
        public RemoveGeneralOutModel RemoveExclusion([FromBody] List<ListInfoWithRowVersion> model)
        {
            var exclusion = new BLL.Calendar.Exclusion.ExclusionGeneral();
            return GeneralCatalogRemove(exclusion, model, "RemoveExclusion");
        }
        #endregion


        #region method GetCalendarWorkSchedule
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarWorkSchedule", Name = "GetCalendarWorkSchedule")]
        public GeneralGetCatalogByIdOutModel GetCalendarWorkScheduleByID([FromBody] GeneralGetCatalogByIdInModel model)
        {
            var calendarWorkSchedule = new BLL.Calendar.CalendarWorkSchedule.CalendarWorkScheduleGeneral();
            return GeneralGetCatalogById(calendarWorkSchedule, model.ID, "GetCalendarWorkSchedule");
        }
        #endregion

        #region method SaveCalendarWorkSchedule

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/SaveCalendarWorkSchedule", Name = "SaveCalendarWorkSchedule")]
        public GeneralCatalogSaveOutModel SaveCalendarWorkSchedule([FromBody] DTL.Calendar.CalendarWorkSchedule model)
        {
            var calendarWorkSchedule = new BLL.Calendar.CalendarWorkSchedule.CalendarWorkScheduleGeneral();
            return GeneralCatalogSave(calendarWorkSchedule, model, "SaveCalendarWorkSchedule");
        }
        #endregion

        #region method RemoveCalendarWorkSchedule
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveCalendarWorkSchedule", Name = "RemoveCalendarWorkSchedule")]
        public RemoveGeneralOutModel RemoveCalendarWorkSchedule([FromBody] List<ListInfoWithRowVersion> model)
        {
            var calendarWorkSchedule = new BLL.Calendar.CalendarWorkSchedule.CalendarWorkScheduleGeneral();
            return GeneralCatalogRemove(calendarWorkSchedule, model, "RemoveCalendarWorkSchedule");
        }
        #endregion

        #region method AddProcessNameList
        public sealed class AddProcessNameListOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        public sealed class AddProcessNameListInModel
        {
            public Guid SoftwareModelID { get; set; }
            public string ProcessNames { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddProcessNameList", Name = "AddProcessNameList")]
        public AddProcessNameListOutModel AddProcessNameList([FromBody] AddProcessNameListInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddProcessNameListOutModel() {Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.AddProcessNameList userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.SoftwareModelID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_SOFTWAREMODEL))
                {
                    Logger.Trace("AssetApiController.AddProcessNameList userID={0}, userName={1} (operation denied)", user.Id, user.UserName);
                    return new AddProcessNameListOutModel() { Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var softwaremodel = BLL.ProductCatalog.Models.SoftwareModelForForm.Get(model.SoftwareModelID, dataSource);
                    softwaremodel.AddProcessNameList(model.ProcessNames, dataSource);
                    //
                    return new AddProcessNameListOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddProcessNameList");
                return new AddProcessNameListOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method RemoveProcessNameList
        public sealed class RemoveProcessNameListOutModel
        {
            public RequestResponceType Result { get; set; }
        }

        public sealed class RemoveProcessNameListInModel
        {
            public Guid SoftwareModelID { get; set; }
            public string ProcessNames { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveProcessNameList", Name = "RemoveProcessNameList")]
        public RemoveProcessNameListOutModel RemoveProcessNameList([FromBody] RemoveProcessNameListInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new RemoveProcessNameListOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.RemoveProcessNameList userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.SoftwareModelID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_SOFTWAREMODEL))
                {
                    Logger.Trace("AssetApiController.RemoveProcessNameList userID={0}, userName={1} (operation denied)", user.Id, user.UserName);
                    return new RemoveProcessNameListOutModel() { Result = RequestResponceType.OperationError };
                }
                using (var dataSource = DataSource.GetDataSource())
                {
                    var softwaremodel = BLL.ProductCatalog.Models.SoftwareModelForForm.Get(model.SoftwareModelID, dataSource);
                    softwaremodel.RemoveProcessNameList(model.ProcessNames, dataSource);
                    //
                    return new RemoveProcessNameListOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RemoveProcessNameList");
                return new RemoveProcessNameListOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

      


        #region AddSoftwareModelDependency
        public sealed class AddSoftwareModelDependencyInModel
        {
            public Guid ParentSoftwareModelID { get; set; }
            public Guid ChildSoftwareModelID { get; set; }
            public int Type { get; set; }

        }
        public sealed class AddSoftwareModelDependencyOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddSoftwareModelDependency", Name = "AddSoftwareModelDependency")]
        public AddSoftwareModelDependencyOutModel AddSoftwareModelDependency([FromBody] AddSoftwareModelDependencyInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddSoftwareModelDependencyOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.AddSoftwareModelDependency userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ParentSoftwareModelID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var softwaremodel = BLL.ProductCatalog.Models.SoftwareModelForForm.Get(model.ParentSoftwareModelID, dataSource);
                    softwaremodel.AddSoftwareModelDependency(model.ParentSoftwareModelID, model.ChildSoftwareModelID, model.Type, dataSource);
                    //
                    return new AddSoftwareModelDependencyOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddSoftwareModelDependency");
                return new AddSoftwareModelDependencyOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region RemoveSoftwareModelDependency
        public sealed class RemoveSoftwareModelDependencyInModel
        {
            public Guid ParentSoftwareModelID { get; set; }
            public Guid ChildSoftwareModelID { get; set; }
            public int Type { get; set; }

        }
        public sealed class RemoveSoftwareModelDependencyOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveSoftwareModelDependency", Name = "RemoveSoftwareModelDependency")]
        public RemoveSoftwareModelDependencyOutModel RemoveSoftwareModelDependency([FromBody] RemoveSoftwareModelDependencyInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new RemoveSoftwareModelDependencyOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.AddSoftwareModelDependency userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ParentSoftwareModelID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var softwaremodel = BLL.ProductCatalog.Models.SoftwareModelForForm.Get(model.ParentSoftwareModelID, dataSource);
                    softwaremodel.RemoveSoftwareModelDependency(model.ParentSoftwareModelID, model.ChildSoftwareModelID, model.Type, dataSource);
                    //
                    return new RemoveSoftwareModelDependencyOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RemoveSoftwareModelDependency");
                return new RemoveSoftwareModelDependencyOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method SaveKBArticleDependencyList
        public sealed class KBArticleDependencyListIn
        {
            public Guid ParentID { get; set; }
            public List<Guid> ChildIDs { get; set; }
        }
        public sealed class KBArticleDependencyListOut
        {
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [Route("sdApi/SaveKBArticleDependencyList", Name = "SaveKBArticleDependencyList")]
        public KBArticleDependencyListOut SaveKBArticleDependencyList(KBArticleDependencyListIn info)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new KBArticleDependencyListOut() { Result = RequestResponceType.AccessError };
                Logger.Trace("FileApiController.SaveKBArticleDependencyList userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {                    
                    var kba = KBArticle.Get(info.ParentID);
                    kba.SaveDependencyObjectList(info.ChildIDs, dataSource);
                }
                return new KBArticleDependencyListOut() { Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения новыых документов в репозиторий.");
                return new KBArticleDependencyListOut() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region GetSynonymsSoftwareModelList
        public sealed class GetSynonymsSoftwareModelListIn
        {
            public Guid ParentID { get; set; }

        }
        public sealed class GetSynonymsSoftwareModelListOut
        {
            public IList<SynonymsSoftwareModel> SynonymsSoftwareModel { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSynonymsSoftwareModelList", Name = "GetSynonymsSoftwareModelList")]
        public GetSynonymsSoftwareModelListOut GetSynonymsSoftwareModelList([FromBody] GetSynonymsSoftwareModelListIn model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetSynonymsSoftwareModelListOut() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetNonSynonymsSoftwareModelList userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ParentID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = SynonymsSoftwareModelForTable.GetSynonymsSoftwareModelList(model.ParentID, dataSource);
                    //
                    return new GetSynonymsSoftwareModelListOut() { SynonymsSoftwareModel = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNonSynonymsSoftwareModelList");
                return new GetSynonymsSoftwareModelListOut() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion


        #region GetNonSynonymsSoftwareModelList
        public sealed class GetNonSynonymsSoftwareModelListIn
        {
            public Guid ParentID { get; set; }

        }
        public sealed class GetNonSynonymsSoftwareModelListOut
        {
            public IList<SynonymsSoftwareModel> NonSynonymsSoftwareModel { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetNonSynonymsSoftwareModelList", Name = "GetNonSynonymsSoftwareModelList")]
        public GetNonSynonymsSoftwareModelListOut GetNonSynonymsSoftwareModelList([FromBody] GetNonSynonymsSoftwareModelListIn model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new GetNonSynonymsSoftwareModelListOut() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetNonSynonymsSoftwareModelList userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ParentID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var retval = SynonymsSoftwareModelForTable.GetNonSynonymsSoftwareModelList(model.ParentID, dataSource);
                    //
                    return new GetNonSynonymsSoftwareModelListOut() { NonSynonymsSoftwareModel = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "GetNonSynonymsSoftwareModelList");
                return new GetNonSynonymsSoftwareModelListOut() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region AddSynonymsSoftwareModel
        public sealed class AddSynonymsSoftwareModelInModel
        {
            public Guid ParentID { get; set; }
            public Guid ChildID { get; set; }

        }
        public sealed class AddSynonymsSoftwareModelOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddSynonymsSoftwareModel", Name = "AddSynonymsSoftwareModel")]
        public  AddSynonymsSoftwareModelOutModel AddSynonymsSoftwareModel([FromBody] AddSynonymsSoftwareModelInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new AddSynonymsSoftwareModelOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.AddSynonymsSoftwareModel userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ParentID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                   SynonymsSoftwareModelForTable.AddSynonymsSoftwareModel(model.ParentID, model.ChildID, dataSource);
                    //
                    return new AddSynonymsSoftwareModelOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddSynonymsSoftwareModel");
                return new AddSynonymsSoftwareModelOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region RemoveSynonymsSoftwareModel
        public sealed class RemoveSynonymsSoftwareModelInModel
        {
            public Guid ParentID { get; set; }
            public Guid ChildID { get; set; }

        }
        public sealed class RemoveSynonymsSoftwareModelOutModel
        {
            public RequestResponceType Result { get; set; }
        }
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveSynonymsSoftwareModel", Name = "RemoveSynonymsSoftwareModel")]
        public RemoveSynonymsSoftwareModelOutModel RemoveSynonymsSoftwareModel([FromBody] RemoveSynonymsSoftwareModelInModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new RemoveSynonymsSoftwareModelOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.RemoveSynonymsSoftwareModel userID={0}, userName={1}, ID={2}", user.Id, user.UserName, model.ParentID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    SynonymsSoftwareModelForTable.RemoveSynonymsSoftwareModel(model.ParentID, model.ChildID, dataSource);
                    return new RemoveSynonymsSoftwareModelOutModel() { Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "RemoveSoftwareModelDependency");
                return new RemoveSynonymsSoftwareModelOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

    }
}


