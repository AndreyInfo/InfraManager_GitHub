using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.SD;
using InfraManager.Web.BLL.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace InfraManager.Web.Controllers.Account
{
    public sealed class UserApiController : BaseApiController
    {
        #region method GetUserInfo
        [HttpGet]
        [Route("userApi/GetUserInfo", Name = "GetUserInfo")]
        [Obsolete("Use api/users/{0} instead")]
        public UserInfo GetUserInfo([FromQuery] Guid userID)
        {
            var currentUser = base.CurrentUser;
            if (currentUser == null)
            {
                Logger.Warning("UserApiController.GetUser userID={0} (currentUser is null)", userID);
                return null;
            }
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var currentUserInfo = UserInfo.Get(currentUser.User.ID, dataSource);
                    if (currentUserInfo == null)
                        throw new NotSupportedException();
                    //
                    var user = UserInfo.Get(userID);
                    if (user == null)
                    {
                        Logger.Trace("UserApiController.GetUser userID={0} failed (user not exists)", userID);
                        return null;
                    }
                    user.CheckCanCall(currentUserInfo);
                    //
                    Logger.Trace("UserApiController.GetUser userID={0}, userName={1}", user.ID, user.Name);
                    return user;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка пользователей из моего подразделения");
                return null;
            }
        }
        #endregion

        #region method GetUserInfoList
        [HttpGet]
        [Route("userApi/GetUserInfoListInMySubdivision", Name = "GetUserInfoListInMySubdivision")]
        public List<UserInfo> GetUserInfoListInMySubdivision()
        {
            var currentUser = base.CurrentUser;
            if (currentUser == null || !currentUser.User.HasRoles)
            {
                Logger.Warning("UserApiController.GetUserInfoListInMySubdivision failed (user is client)");
                return null;//guard
            }
            //
            try
            {
                var retval = new List<UserInfo>();
                using (var dataSource = DataSource.GetDataSource())
                {
                    var currentUserInfo = UserInfo.Get(currentUser.User.ID, dataSource);
                    if (currentUserInfo != null && currentUserInfo.SubdivisionID.HasValue)
                    {
                        retval = UserInfo.GetListBySubdivision(currentUserInfo.SubdivisionID.Value, dataSource);
                        retval.ForEach(x => x.CheckCanCall(currentUserInfo));
                    }
                }
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка пользователей из моего подразделения");
                return null;
            }
        }
        #endregion

        #region method GetUserInfoListOnCustomControl
        [HttpGet]
        [Route("userApi/GetUserInfoListOnCustomControl", Name = "GetUserInfoListOnCustomControl")]
        public List<UserInfo> GetUserInfoListOnCustomControl(Guid objectID, int objectClassID)
        {
            return new List<UserInfo>();
            var user = base.CurrentUser;
            //
            try
            {
                var retval = new List<UserInfo>();
                using (var dataSource = DataSource.GetDataSource())
                {
                    if (objectClassID == IMSystem.Global.OBJ_CALL)
                    {
                        if (!BLL.SD.Calls.Call.AccessIsGranted(objectID, user.User, true, dataSource))
                        {
                            Logger.Error("UserApiController.GetUserInfoListOnCustomControl userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, objectID, objectClassID);
                            return null;
                        }
                    }
                    else if (objectClassID == IMSystem.Global.OBJ_WORKORDER)
                    {
                        if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(objectID, user.User, true, dataSource))
                        {
                            Logger.Error("UserApiController.GetUserInfoListOnCustomControl userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, objectID, objectClassID);
                            return null;
                        }
                    }
                    else if (objectClassID == IMSystem.Global.OBJ_PROBLEM)
                    {
                        if (!BLL.SD.Problems.Problem.AccessIsGranted(objectID, user.User, true, dataSource))
                        {
                            Logger.Error("UserApiController.GetUserInfoListOnCustomControl userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, objectID, objectClassID);
                            return null;
                        }
                    }
                    else if (objectClassID == IMSystem.Global.OBJ_RFC)
                    {
                        if (!BLL.SD.RFC.RFC.AccessIsGranted(objectID, user.User, true, dataSource))
                        {
                            Logger.Error("UserApiController.GetUserInfoListOnCustomControl userID={0}, userName={1}, objID={2}, objClassId={3} failed (access denied)", user.Id, user.UserName, objectID, objectClassID);
                            return null;
                        }
                    }
                    else
                        throw new NotSupportedException("objectClassID not valid");
                    //
                    retval = UserInfo.GetListOnCustomControl(objectID, objectClassID, dataSource);
                }
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка пользователей из моего подразделения");
                return null;
            }
        }
        #endregion

        #region method GetQueueInfo
        [HttpGet]
        [Route("userApi/GetQueueInfo", Name = "GetQueueInfo")]
        public QueueInfo GetQueueInfo([FromQuery] Guid queueID)
        {
            return new QueueInfo(new DTL.SD.QueueInfo { ID = queueID });

            var currentUser = base.CurrentUser;
            if (currentUser == null)
            {
                Logger.Warning("UserApiController.GetQueueInfo queueID={0} failed (user is client)", queueID);
                return null;//guard
            }
            //
            var queue = QueueInfo.Get(queueID);
            if (queue == null)
            {
                Logger.Trace("UserApiController.GetQueueInfo queueID={0} failed (user not exists)", queueID);
                return null;
            }
            //
            Logger.Trace("UserApiController.GetQueueInfo queueID={0}, queueName={1}", queue.ID, queue.Name);
            return queue;
        }
        #endregion

        #region method GetOrganizationInfo
        [HttpGet]
        [Route("userApi/GetOrganizationInfo", Name = "GetOrganizationInfo")]
        public string GetOrganizationInfo([FromQuery] Guid organizationID)
        {
            var currentUser = base.CurrentUser;
            if (currentUser == null)
            {
                Logger.Warning("UserApiController.GetOrganizationInfo organizationID={0} failed (user is client)", organizationID);
                return null;
            }
            //
            string organizationName = string.Empty;
            using (var dataSource = DataSource.GetDataSource())
                organizationName = UserInfo.GetOrganizationName(organizationID, dataSource);
            //
            Logger.Trace("UserApiController.GetOrganizationInfo organizationID={0}, organizationName={1}", organizationID, organizationName);
            return organizationName;
        }
        #endregion

        #region method GetSubdivistionInfo
        [HttpGet]
        [Route("userApi/GetSubdivistionInfo", Name = "GetSubdivistionInfo")]
        public string GetSubdivistionInfo([FromQuery] Guid subdivistionID)
        {
            var currentUser = base.CurrentUser;
            if (currentUser == null)
            {
                Logger.Warning("UserApiController.GetSubdivistionInfo subdivistionID={0} failed (user is client)", subdivistionID);
                return null;
            }
            //
            string subdivistionName = string.Empty;
            using (var dataSource = DataSource.GetDataSource())
                subdivistionName = UserInfo.GetSubdivisionName(subdivistionID, dataSource);
            //
            Logger.Trace("UserApiController.GetSubdivistionInfo subdivistionID={0}, subdivistionName={1}", subdivistionID, subdivistionName);
            return subdivistionName;
        }
        #endregion

        #region method GetContactPersonInfo
        [HttpGet]
        [Route("userApi/GetContactPersonInfo", Name = "GetContactPersonInfo")]
        public UserInfo GetContactPersonInfo([FromQuery] Guid userID)
        {
            var currentUser = base.CurrentUser;
            if (currentUser == null)
            {
                Logger.Warning("UserApiController.GetContactPerson userID={0} (currentUser is null)", userID);
                return null;
            }
            try
            {
                using (var dataSource = DataSource.GetDataSource())
                {
                    var currentContactPersonInfo = UserInfo.Get(currentUser.User.ID, dataSource);
                    if (currentContactPersonInfo == null)
                        throw new NotSupportedException();
                    //
                    var user = UserInfo.GetContactPersonInfo(userID);
                    if (user == null)
                    {
                        Logger.Trace("UserApiController.GetContactPerson userID={0} failed (user not exists)", userID);
                        return null;
                    }
                    user.CheckCanCall(currentContactPersonInfo);
                    //
                    Logger.Trace("UserApiController.GetContactPerson userID={0}, userName={1}", user.ID, user.Name);
                    return user;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка контактных лиц");
                return null;
            }
        }
        #endregion
    }
}
