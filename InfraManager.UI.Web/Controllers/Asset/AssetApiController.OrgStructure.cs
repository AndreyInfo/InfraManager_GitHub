using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL;
using InfraManager.Web.BLL.Assets.OrgStructure;
using InfraManager.Web.BLL.Tables;
using InfraManager.Web.DTL.Tables;
using InfraManager.Web.Helpers;
using System;
using System.Collections.Generic;
using InfraManager.ResourcesArea;
using Microsoft.AspNetCore.Mvc;
using InfraManager.UI.Web.ModelBinding;

namespace InfraManager.Web.Controllers.IM
{
    public partial class AssetApiController
    {
        public sealed class ObjectModel
        {
            public int ObjectClassID { get; set; }
            public Guid ObjectID { get; set; }
        }

        #region method GetOwner
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetOwner", Name = "GetOwner")]
        public ResultData<Owner> GetOwner([FromQuery] ObjectModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultData<Owner>.Create(RequestResponceType.NullParamsError);
            if (model.ObjectClassID != IMSystem.Global.OBJ_OWNER)
                return ResultData<Owner>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.GetOwner userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ObjectClassID, model.ObjectID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_OWNER))
                {
                    Logger.Trace("AssetApiController.GetOwner userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ObjectID);
                    return ResultData<Owner>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    var owner = Owner.Get(dataSource);
                    return ResultData<Owner>.Create(RequestResponceType.Success, owner);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Owner>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method EditOwner
        [HttpPost]
        [Route("assetApi/EditOwner", Name = "EditOwner")]
        public ResultData<Owner> EditOwner(DTL.Assets.Owner model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new ResultData<Owner>(RequestResponceType.NullParamsError, null);
            if (model.ClassID != IMSystem.Global.OBJ_OWNER)
                return ResultData<Owner>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.EditOwner userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ClassID, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_OWNER))
                {
                    Logger.Trace("AssetApiController.EditSubdivision userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return ResultData<Owner>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    try
                    {
                        var owner = Owner.Update(model, dataSource);
                        dataSource.CommitTransaction();

                        WorkflowWrapper.MakeOnSaveReaction(model.ID, model.ClassID, dataSource, user.User);
                        return new ResultData<Owner>(RequestResponceType.Success, owner);
                    }
                    catch 
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Owner>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Owner>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Owner>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return ResultData<Owner>.Create(RequestResponceType.ValidationError); 
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Owner>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method GetOrganization
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetOrganization", Name = "GetOrganization")]
        public ResultData<Organization> GetOrganization([FromQuery] ObjectModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultData<Organization>.Create(RequestResponceType.NullParamsError);
            if (model.ObjectClassID != IMSystem.Global.OBJ_ORGANIZATION)
                return ResultData<Organization>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.GetOrganization userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ObjectClassID, model.ObjectID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_ORGANIZATION))
                {
                    Logger.Trace("AssetApiController.GetOrganization userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ObjectID);
                    return ResultData<Organization>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    var organization = Organization.Get(model.ObjectID, dataSource);
                    return ResultData<Organization>.Create(RequestResponceType.Success, organization);
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method AddOrganization
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddOrganization", Name = "AddOrganization")]
        public ResultData<Organization> AddOrganization([FromBodyOrForm] DTL.Assets.Organization model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new ResultData<Organization>(RequestResponceType.NullParamsError, null);
            if (model.ClassID != IMSystem.Global.OBJ_ORGANIZATION)
                return ResultData<Organization>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.AddOrganization userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ClassID, model.ID);

            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_ORGANIZATION))
                {
                    Logger.Trace("AssetApiController.AddOrganization userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return ResultData<Organization>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    try
                    {
                        var organization = Organization.Add(model, dataSource);
                        dataSource.CommitTransaction();

                        WorkflowWrapper.MakeOnSaveReaction(organization.ID, organization.ClassID, dataSource, user.User);
                        return new ResultData<Organization>(RequestResponceType.Success, organization);
                    }
                    catch
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }                
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return ResultData<Organization>.Create(RequestResponceType.ValidationError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method EditOrganization
        [HttpPost]
        [Route("assetApi/EditOrganization", Name = "EditOrganization")]
        public ResultData<Organization> EditOrganization([FromBodyOrForm]DTL.Assets.Organization model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new ResultData<Organization>(RequestResponceType.NullParamsError, null);
            if (model.ClassID != IMSystem.Global.OBJ_ORGANIZATION)
                return ResultData<Organization>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.EditOrganization userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ClassID, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_ORGANIZATION))
                {
                    Logger.Trace("AssetApiController.EditOrganization userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return ResultData<Organization>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    try
                    {
                        var organization = Organization.Update(model, dataSource);
                        dataSource.CommitTransaction();

                        WorkflowWrapper.MakeOnSaveReaction(organization.ID, organization.ClassID, dataSource, user.User);
                        return new ResultData<Organization>(RequestResponceType.Success, organization);
                    }
                    catch
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }                
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return ResultData<Organization>.Create(RequestResponceType.ValidationError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Organization>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method GetSubdivision
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSubdivision", Name = "GetSubdivision")]
        public ResultData<Subdivision> GetSubdivision([FromQuery] ObjectModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultData<Subdivision>.Create(RequestResponceType.NullParamsError);
            if (model.ObjectClassID != IMSystem.Global.OBJ_DIVISION)
                return ResultData<Subdivision>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.GetSubdivision userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ObjectClassID, model.ObjectID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_PROPERTIES_SUBDIVISION))
                {
                    Logger.Trace("AssetApiController.GetSubdivision userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ObjectID);
                    return ResultData<Subdivision>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    var subdivision = Subdivision.Get(model.ObjectID, dataSource);
                    return ResultData<Subdivision>.Create(RequestResponceType.Success, subdivision);
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method AddSubdivision
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/AddSubdivision", Name = "AddSubdivision")]
        public ResultData<Subdivision> AddSubdivision([FromBodyOrForm] DTL.Assets.Subdivision model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new ResultData<Subdivision>(RequestResponceType.NullParamsError, null);
            if (model.ClassID != IMSystem.Global.OBJ_DIVISION)
                return ResultData<Subdivision>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.AddSubdivision userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ClassID, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_ADD_SUBDIVISION))
                {
                    Logger.Trace("AssetApiController.AddSubdivision userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return ResultData<Subdivision>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    try
                    {
                        var subdivision = Subdivision.Add(model, dataSource);
                        dataSource.CommitTransaction();

                        WorkflowWrapper.MakeOnSaveReaction(subdivision.ID, subdivision.ClassID, dataSource, user.User);
                        return new ResultData<Subdivision>(RequestResponceType.Success, subdivision);
                    }
                    catch
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }                
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return ResultData<Subdivision>.Create(RequestResponceType.ValidationError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при добавлении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method EditSubdivision
        [HttpPost]
        [Route("assetApi/EditSubdivision", Name = "EditSubdivision")]
        public ResultData<Subdivision> EditSubdivision([FromBodyOrForm] DTL.Assets.Subdivision model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new ResultData<Subdivision>(RequestResponceType.NullParamsError, null);
            if (model.ClassID != IMSystem.Global.OBJ_DIVISION)
                return ResultData<Subdivision>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.EditSubdivision userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ClassID, model.ID);
            try
            {
                if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_UPDATE_SUBDIVISION))
                {
                    Logger.Trace("AssetApiController.EditSubdivision userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ID);
                    return ResultData<Subdivision>.Create(RequestResponceType.OperationError);
                }

                using (var dataSource = DataSource.GetDataSource())
                {
                    dataSource.BeginTransaction();
                    try
                    {
                        var subdivision = Subdivision.Update(model, dataSource);
                        dataSource.CommitTransaction();

                        WorkflowWrapper.MakeOnSaveReaction(subdivision.ID, subdivision.ClassID, dataSource, user.User);
                        return new ResultData<Subdivision>(RequestResponceType.Success, subdivision);
                    }
                    catch
                    {
                        dataSource.RollbackTransaction();
                        throw;
                    }                
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (ArgumentValidationException ex)
            {
                Logger.Warning(ex);
                return ResultData<Subdivision>.Create(RequestResponceType.ValidationError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при изменении элемента оргструктуры.");
                return ResultData<Subdivision>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method RemoveOrgStructureObject
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/RemoveOrgStructureObject", Name = "RemoveOrgStructureObject")]
        public ResultWithMessage RemoveOrgStructureObject([FromBodyOrForm]ObjectModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultWithMessage.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("AssetApiController.RemoveOrgStructureObject userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (model.ObjectClassID == IMSystem.Global.OBJ_ORGANIZATION)
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_DELETE_ORGANIZATION))
                        {
                            Logger.Trace("AssetApiController.RemoveOrgStructureObject userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ObjectID);
                            return ResultWithMessage.Create(RequestResponceType.OperationError);
                        }

                        Organization.Remove(model.ObjectID, dataSource);
                    }
                    else if (model.ObjectClassID == IMSystem.Global.OBJ_DIVISION)
                    {
                        if (!user.User.OperationIsGranted(IMSystem.Global.OPERATION_DELETE_SUBDIVISION))
                        {
                            Logger.Trace("AssetApiController.RemoveOrgStructureObject userID={0}, userName={1}, ID={2} failed (operation denied)", user.Id, user.UserName, model.ObjectID);
                            return ResultWithMessage.Create(RequestResponceType.OperationError);
                        }

                        Subdivision.Remove(model.ObjectID, dataSource);
                    }
                    else
                        throw new NotSupportedException("model.ObjectClassID");

                    WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                    return ResultWithMessage.Create(RequestResponceType.Success);
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при удалении элемента оргструктуры.");
                return ResultWithMessage.Create(RequestResponceType.ObjectDeleted, Resources.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при удалении элемента оргструктуры.");
                return ResultWithMessage.Create(RequestResponceType.OperationError, Resources.ObjectInUse);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при удалении элемента оргструктуры.");
                return ResultWithMessage.Create(RequestResponceType.ConcurrencyError, Resources.ObjectDeleted);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при удалении элемента оргструктуры.");
                return ResultWithMessage.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method GetSubdivisionListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetSubdivisionListForTable", Name = "GetSubdivisionListForTable")]
        public ResultData<List<BaseForTable>> GetSubdivisionListForTable([FromBodyOrForm] OrgStructureSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.SubdivisionList);
        }
        #endregion

        #region method GetOrganizationListForTable
        //for ko.listView
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("assetApi/GetOrganizationListForTable", Name = "GetOrganizationListForTable")]
        public ResultData<List<BaseForTable>> GetOrganizationListForTable([FromBodyOrForm] OrgStructureSearchTableLoadRequestInfo requestInfo)
        {
            var user = base.CurrentUser;
            return TableHelper.GetListForObject(requestInfo, user, TableHelper.TableType.OrganizationList);
        }
        #endregion

        #region method GetAllOrganizationList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetAllOrganizationList", Name = "GetAllOrganizationList")]
        public ResultData<List<Organization>> GetAllOrganizationList()
        {
            var user = base.CurrentUser;
            if (user == null)
                return new ResultData<List<Organization>>(RequestResponceType.NullParamsError, null);
            //
            Logger.Trace("AssetApiController.GetAllOrganizationList userID={0}, userName={1}",
                user.Id, user.UserName);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var organizationList = Organization.GetList(dataSource);
                    return ResultData<List<Organization>>.Create(RequestResponceType.Success, organizationList);
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Organization>>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Organization>>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Organization>>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Organization>>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method GetOrganizationSubdivisionList
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetSubdivisionListByOrganization", Name = "GetSubdivisionListByOrganization")]
        public ResultData<List<Subdivision>> GetSubdivisionListByOrganization([FromQuery] ObjectModel model)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new ResultData<List<Subdivision>>(RequestResponceType.NullParamsError, null);
            if (model.ObjectClassID != IMSystem.Global.OBJ_ORGANIZATION)
                return ResultData<List<Subdivision>>.Create(RequestResponceType.BadParamsError);
            //
            Logger.Trace("AssetApiController.GetSubdivisionListByOrganization userID={0}, userName={1}, ObjectClassID={2}, ObjectID={3}",
                user.Id, user.UserName, model.ObjectClassID, model.ObjectID);
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var subdivisionList = Subdivision.GetList(model.ObjectID, dataSource);
                    return ResultData<List<Subdivision>>.Create(RequestResponceType.Success, subdivisionList);
                }
            }
            catch (ObjectDeletedException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Subdivision>>.Create(RequestResponceType.ObjectDeleted);
            }
            catch (ObjectInUseException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Subdivision>>.Create(RequestResponceType.OperationError);
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Subdivision>>.Create(RequestResponceType.ConcurrencyError);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<Subdivision>>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

        #region method CalendarWorkSchedule
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("assetApi/GetCalendarWorkScheduleList", Name = "GetCalendarWorkScheduleList")]
        public ResultData<List<CalendarWorkSchedule>> GetCalendarWorkScheduleList()
        {
            var user = base.CurrentUser;
            if (user == null)
                return ResultData<List<CalendarWorkSchedule>>.Create(RequestResponceType.NullParamsError);
            //
            Logger.Trace("AssetApiController.GetCalendarWorkScheduleList userID={0}, userName={1}", user.Id, user.UserName);
            try
            {
                var calendarWorkScheduleList = CalendarWorkSchedule.GetList();
                return ResultData<List<CalendarWorkSchedule>>.Create(RequestResponceType.Success, calendarWorkScheduleList);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка при получении элемента оргструктуры.");
                return ResultData<List<CalendarWorkSchedule>>.Create(RequestResponceType.GlobalError);
            }
        }
        #endregion

    }
}
