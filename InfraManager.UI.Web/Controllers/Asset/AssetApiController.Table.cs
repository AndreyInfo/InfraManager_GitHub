using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL.Assets;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.Web.BLL.Assets.AssetNumber;
using InfraManager.Web.BLL.FormBuilder;

namespace InfraManager.Web.Controllers.IM
{

    public partial class AssetApiController
    {
        #region (Table methods)
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("imApi/GetListForTable", Name = "IMGetListForTable")]
        public TableHelper.GetTableOutModel GetListForTable(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForTable(requestInfo, user, TableHelper.TableType.Asset);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetListForAssetObject", Name = "GetListForAssetObject")]
        public ResultData<List<BaseForTable>> GetListForAssetObject(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            var response = TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Asset);
            return response;
        }
        #endregion

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetListForSearchAssetObject", Name = "GetListForSearchAssetObject")]
        public ResultData<List<BaseForTable>> GetListForSearchAssetObject(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.AssetUsersList);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetContractAgreementObject", Name = "GetContractAgreementObject")]
        public ResultData<List<BaseForTable>> GetContractAgreementObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.ContractAgreement);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetAssetMaintenanceObject", Name = "GetAssetMaintenanceObject")]
        public ResultData<List<BaseForTable>> GetAssetMaintenanceObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.ContractAssetMaintenance);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetAssetMaintenanceObjectForAgreement", Name = "GetAssetMaintenanceObjectForAgreement")]
        public ResultData<List<BaseForTable>> GetAssetMaintenanceObjectForAgreement(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.AgreementAssetMaintenance);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetLicenceMaintenanceObject", Name = "GetLicenceMaintenanceObject")]
        public ResultData<List<BaseForTable>> GetLicenceMaintenanceObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.ContractLicenceMaintenance);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetLicenceMaintenanceObjectForAgreement", Name = "GetLicenceMaintenanceObjectForAgreement")]
        public ResultData<List<BaseForTable>> GetLicenceMaintenanceObjectForAgreement(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.AgreementLicenceMaintenance);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSupplierObject", Name = "GetSupplierObject")]
        public ResultData<List<BaseForTable>> GetSupplierObject(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Supplier);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetDeputyObject", Name = "GetDeputyObject")]
        public ResultData<List<BaseForTable>> GetDeputyObject([FromBodyOrForm] TableLoadRequestInfoDeputy requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Deputy);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSupplierConractPersonObject", Name = "GetSupplierConractPersonObject")]
        public ResultData<List<BaseForTable>> GetSupplierConractPersonObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SupplierContactPerson);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareLicenceObjectForTable", Name = "GetSoftwareLicenceObjectForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareLicenceObject(AssetModelSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SoftwareLicence);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareSubLicenceObject", Name = "GetSoftwareSubLicenceObject")]
        public ResultData<List<BaseForTable>> GetSoftwareSubLicenceObject(TableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Asset);
        }


        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelObject", Name = "GetSoftwareModelObject")]
        public ResultData<List<BaseForTable>> GetSoftwareModelObject(AssetModelSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SoftModel);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareLicenceSerialNumberObject", Name = "GetSoftwareLicenceSerialNumberObject")]
        public ResultData<List<BaseForTable>> GetSoftwareLicenceSerialNumberObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareLicenceSerialNumber);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareLicenceUpdateObject", Name = "GetSoftwareLicenceUpdateObject")]
        public ResultData<List<BaseForTable>> GetSoftwareLicenceUpdateObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareLicenceUpdate);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetDataEntityDependencyListForTable", Name = "GetDataEntityDependencyListForTable")]
        public ResultData<List<BaseForTable>> GetDataEntityDependencyListForTable(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.DataEntityDependency);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetClusterVMForTable", Name = "GetClusterVMForTable")]
        public ResultData<List<BaseForTable>> GetClusterVMForTable(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.ClusterVM);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetClusterHostsForTable", Name = "GetClusterHostsForTable")]
        public ResultData<List<BaseForTable>> GetClusterHostsForTable(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.ClusterHosts);
        }
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareLicenceReferenceObject", Name = "GetSoftwareLicenceReferenceObject")]
        public ResultData<List<BaseForTable>> GetSoftwareLicenceReferenceObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareLicenceReference);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareLicenceReference", Name = "GetSoftwareLicenceReference")]
        public ResultData<List<BaseForTable>> GetSoftwareLicenceReference(frmTableLoadRequestInfoForPool requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SoftwareLicenceReferenceReturning);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetAdapterReferenceObject", Name = "GetAdapterReferenceObject")]
        public ResultData<List<BaseForTable>> GetAdapterReferenceObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.AdapterReference);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetPeripheralReferenceObject", Name = "GetPeripheralReferenceObject")]
        public ResultData<List<BaseForTable>> GetPeripheralReferenceObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.PeripheralReference);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSubdeviceList", Name = "GetSubdeviceList")]
        public ResultData<List<BaseForTable>> GetSubdeviceList(SimpleRequestWithSearchAndNavigatorInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SubdeviceList);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetUsersList", Name = "GetUsersList")]
        public ResultData<List<BaseForTable>> GetUsersList(SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Users);
        }


        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetEquipList", Name = "GetEquipList")]
        public ResultData<List<BaseForTable>> GetEquipList(SimpleRequestWithSearchAndNavigatorInfoAndSoftwareSubLicence requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Equip);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetAssetSearchObject", Name = "GetAssetSearchObject")]
        public ResultData<List<BaseForTable>> GetAssetSearchObject(AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.AssetSearch);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetContractLicenceObject", Name = "GetContractLicenceObject")]
        public ResultData<List<BaseForTable>> GetContractLicenceObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.ContractLicence);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetContractLicenceAgreementObject", Name = "GetContractLicenceAgreementObject")]
        public ResultData<List<BaseForTable>> GetContractLicenceAgreementObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.ContractLicenceAgreement);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetInventorySpecificationObject", Name = "GetInventorySpecificationObject")]
        public ResultData<List<BaseForTable>> GetInventorySpecificationObject(AssetSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.InventorySpecification);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelUsingTypeObject", Name = "GetSoftwareModelUsingTypeObject")]
        public ResultData<List<BaseForTable>> GetSoftwareModelUsingTypeObject([FromBodyOrForm] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            if (string.IsNullOrEmpty(requestInfo.ViewName))
                requestInfo.ViewName = TableHelper.TableType.SoftwareModelUsingType.ToString();
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SoftwareModelUsingType);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareLicenceLocationObject", Name = "GetSoftwareLicenceLocationObject")]
        public ResultData<List<BaseForTable>> GetSoftwareLicenceLocationObject(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareLicenceLocation);
        }

        #region GetManufacturer
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetManufacturer", Name = "GetManufacturer")]
        public ResultData<List<BaseForTable>> GetManufacturer([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetManufacturer userID={user.Id}, userName={user.UserName}");
            //
            if (string.IsNullOrEmpty(requestInfo.ViewName))
                requestInfo.ViewName = TableHelper.TableType.Manufacturer.ToString();
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Manufacturer);
        }
        #endregion

        #region GetSynonymsForTable
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSynonymsForTable", Name = "assetApi/GetSynonymsForTable")]
        public ResultData<List<BaseForTable>> GetSynonymsForTable([FromBody] SimpleRequestWithSearchInfoAndFilterSynonym requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetSynonymsForTable userID={user.Id}, userName={user.UserName}");
            //
            int? directoryID;
            using (var dataSource = DataSource.GetDataSource())
            {
                BLL.Synonyms.Synonym.CanUseTableForSynonyms(requestInfo.TableName, dataSource, out directoryID);
            }
            if (!directoryID.HasValue)
            {
                Logger.Trace($"Для данной таблицы синонимы не включены");
                return ResultData<List<BaseForTable>>.Create(RequestResponceType.SynonymNotEnabled, null);
            }
            requestInfo.DirectoryID = directoryID.Value;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Synonym);
        }
        #endregion

        #region GetPositions
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetPositions", Name = "GetPositions")]
        public ResultData<List<BaseForTable>> GetPositions([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetPositions userID={user.Id}, userName={user.UserName}");
            //
            if (string.IsNullOrEmpty(requestInfo.ViewName))
                requestInfo.ViewName = TableHelper.TableType.Positions.ToString();
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Positions);
        }
        #endregion

        #region GetUnits
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetUnits", Name = "GetUnits")]
        public ResultData<List<BaseForTable>> GetUnits([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetUnits userID={user.Id}, userName={user.UserName}");
            //
            if (string.IsNullOrEmpty(requestInfo.ViewName))
                requestInfo.ViewName = TableHelper.TableType.Units.ToString();
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Units);
        }
        #endregion

        #region GetCriticality
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCriticality", Name = "GetCriticality")]
        public ResultData<List<BaseForTable>> GetCriticality([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetCriticality userID={user.Id}, userName={user.UserName}");
            //
            if (string.IsNullOrEmpty(requestInfo.ViewName))
                requestInfo.ViewName = TableHelper.TableType.Criticality.ToString();
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Criticality);
        }
        #endregion

        #region GetCriticality
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetInfrastructureSegment", Name = "InfrastructureSegment")]
        public ResultData<List<BaseForTable>> GetInfrastructureSegment([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetCriticality userID={user.Id}, userName={user.UserName}");
            //
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.InfrastructureSegment);
        }
        #endregion

        #region GetFileSystemObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetFileSystemObject", Name = "GetFileSystemObject")]
        public ResultData<List<BaseForTable>> GetFileSystemObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetFileSystemObject userID={user.Id}, userName={user.UserName}");
            //
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.FileSystem);
        }
        #endregion

        #region GetCostCategoryObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCostCategoryObject", Name = "GetCostCategoryObject")]
        public ResultData<List<BaseForTable>> GetCostCategoryObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace($"assetApi.GetCostCategoryObject userID={user.Id}, userName={user.UserName}");
            //
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.CostCategory);
        }
        #endregion

        #region GetRFCCategoryListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetRFCCategoryListForTable", Name = "GetRFCCategoryListForTable")]
        public ResultData<List<BaseForTable>> GetRFCCategoryListForTable(SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            if (string.IsNullOrEmpty(requestInfo.ViewName))
                requestInfo.ViewName = TableHelper.TableType.RFCCategory.ToString();
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.RFCCategory);
        }
        #endregion

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelUpdateForTable", Name = "GetSoftwareModelUpdateForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareModelUpdateForTable([FromBodyOrForm] frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareModelUpdate);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelComponentForTable", Name = "GetSoftwareModelComponentForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareModelComponentForTable([FromBody] frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareModelСomponent);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelPackageContentsForTable", Name = "GetSoftwareModelPackageContentsForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareModelPackageContentsForTable([FromBody] frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareModelPackageContents);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetProcessNameListForTable", Name = "GetProcessNameListForTable")]
        public ResultData<List<BaseForTable>> GetProcessNameListForTable([FromBody] frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareModelProcessNameList);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelInstallationForTable", Name = "GetSoftwareModelInstallationForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareModelInstallationForTable([FromBody] frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareModelInstallation);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelLicensesForTable", Name = "GetSoftwareModelLicensesForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareModelLicensesForTable([FromBodyOrForm] frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SoftwareModelLicenses);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetLogicObjectComponentsForTable", Name = "GetLogicObjectComponentsForTable")]
        public ResultData<List<BaseForTable>> GetLogicObjectComponentsForTable(frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.LogicObjectComponents);
        }

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSynonymsModelForTable", Name = "GetSynonymsModelForTable")]
        public ResultData<List<BaseForTable>> GetSynonymsModelForTable([FromBodyOrForm] frmTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as frmTableLoadRequestInfo, user, TableHelper.TableType.SynonymsSoftwareModelForTable);
        }

        #region GetTelephoneTypeListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetTelephoneTypeListForTable", Name = "GetTelephoneTypeListForTable")]
        public ResultData<List<BaseForTable>> GetTelephoneTypeListForTable([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.TelephoneType);
        }
        #endregion

        //====

        #region GetTechnologyTypeListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetTechnologyTypeListForTable", Name = "GetTechnologyTypeListForTable")]
        public ResultData<List<BaseForTable>> GetTechnologyTypeListForTable([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            if (string.IsNullOrEmpty(requestInfo.ViewName))
                requestInfo.ViewName = TableHelper.TableType.TechnologyType.ToString();
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.TechnologyType);
        }
        #endregion

        #region GetCartridgeTypeListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCartridgeTypeListForTable", Name = "GetCartridgeTypeListForTable")]
        public ResultData<List<BaseForTable>> GetCartridgeTypeListForTable([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.CartridgeType);
        }
        #endregion

        #region GetConnectorTypeListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetConnectorTypeListForTable", Name = "GetConnectorTypeListForTable")]
        public ResultData<List<BaseForTable>> GetConnectorTypeListForTable([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.ConnectorType);
        }
        #endregion

        #region GetSlotTypeListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSlotTypeListForTable", Name = "GetSlotTypeListForTable")]
        public ResultData<List<BaseForTable>> GetSlotTypeListForTable([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SlotType);
        }
        #endregion

        #region method GetSoftwareModelCatalog
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelCatalog", Name = "GetSoftwareModelCatalog")]
        public ResultData<List<BaseForTable>> GetSoftwareModelCatalog([FromBody] TableLoadRequestInfoSoftwareModelCatalog requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as TableLoadRequestInfoSoftwareModelCatalog, user, TableHelper.TableType.SoftwareModelCatalog);
        }
        #endregion

        #region method GetSoftwareModelCatalogDependency
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelCatalogDependency", Name = "GetSoftwareModelCatalogDependency")]
        public ResultData<List<BaseForTable>> GetSoftwareModelCatalogDependency([FromBody] TableLoadRequestInfoSoftwareModelCatalog requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as TableLoadRequestInfoSoftwareModelCatalog, user, TableHelper.TableType.SoftwareModelCatalogDependency);
        }
        #endregion

        #region method GetSoftwareModelRelatedForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSoftwareModelRelatedForTable", Name = "GetSoftwareModelRelatedForTable")]
        public ResultData<List<BaseForTable>> GetSoftwareModelRelatedForTable([FromBody] TableLoadRequestInfoSoftwareModelCatalog requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo as TableLoadRequestInfoSoftwareModelCatalog, user, TableHelper.TableType.SoftwareModelRelated);
        }
        #endregion

        #region method GetWorkflow
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetWorkflowScheme", Name = "GetWorkflowScheme")]
        public ResultData<List<BaseForTable>> GetWorkflowScheme([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            //
            //Logger.Trace($"assetApi.GetWorkflow userID={user.Id}, userName={user.UserName}");
            //
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Workflow);
        }
        #endregion

        #region method GetCalendarHolidayObject
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarHolidayObject", Name = "GetCalendarHolidayObject")]
        public ResultData<List<BaseForTable>> GetCalendarHolidayObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.CalendarHoliday);
        }
        #endregion

        #region method GetCalendarWeekendObject
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarWeekendObject", Name = "GetCalendarWeekendObject")]
        public ResultData<List<BaseForTable>> GetCalendarWeekendObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.CalendarWeekend);
        }
        #endregion

        #region method GetFormBuilderObject

        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetFormBuilderObject", Name = "GetFormBuilderObject")]
        public ResultData<List<BaseForTable>> GetFormBuilderObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.FormBuilder);
        }
        #endregion

        #region method GetExclusionObject
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetExclusionObject", Name = "GetExclusionObject")]
        public ResultData<List<BaseForTable>> GetExclusionObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.Exclusion);
        }
        #endregion

        #region GetParameterEnumObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetParameterEnumObject", Name = "GetParameterEnumObject")]
        public ResultData<List<BaseForTable>> GetParameterEnumObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.ParameterEnum);
        }
        #endregion

        #region GetCalendarWorkScheduleObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetCalendarWorkScheduleObject", Name = "GetCalendarWorkScheduleObject")]
        public ResultData<List<BaseForTable>> GetCalendarWorkScheduleObject([FromBody] SimpleRequestWithSearchInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.CalendarWorkSchedule);
        }
        #endregion
    }
}
