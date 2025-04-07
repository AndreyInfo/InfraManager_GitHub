using System;
using InfraManager.Core.Logging;
using InfraManager.Web.Controllers;
using InfraManager.BLL.Users;
using InfraManager.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static InfraManager.Web.Scripts.models.ClientSearch.ClientPosition;

namespace InfraManager.UI.Web.Controllers.Controls
{
    public class ClientInfoApiController : BaseApiController
    {
        private readonly IUserBLL _users;

        public ClientInfoApiController(IUserBLL users)
        {
            _users = users;
        }
        
        public sealed class ClientNewInfo
        {
            public RequestResponceType Result { get; set; }
            public string Message { get; set; }
        }
        
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("clientInfoApi/postNewClientInfo", Name = "postNewClientInfo")]
        public ClientNewInfo NewClientInfo(ClientNewInfoModel model)
        {
            try
            {
                var user = CurrentUser;
                if (user == null)
                {
                    return new ClientNewInfo { Result = RequestResponceType.NullParamsError };
                }
                
                Logger.Trace("GetClientLocationInfo userID={0}, userName={1}", user.Id, user.UserName);

                if (PostNewClientInfo(model))
                {
                    return new ClientNewInfo { Result = RequestResponceType.Success };
                }
                
                return new ClientNewInfo { Result = RequestResponceType.OperationError };
            }
            catch (ObjectConcurrencyException ex)
            {
                Logger.Error(ex);
                return new ClientNewInfo { Result = RequestResponceType.BadParamsError, Message = "UserDataHasBeenChange" };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new ClientNewInfo { Result = RequestResponceType.GlobalError };
            }
        }
        
        public sealed class ClientLocation
        {
            public СlientLocationInfo ClientLocationInfo { get; set; }
            public RequestResponceType Result { get; set; }
        }
        
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("clientInfoApi/getClientLocationInfo", Name = "getClientLocationInfo")]
        public ClientLocation GetClientLocation(ClientLocationModel param)
        {
            try
            {
                var user = CurrentUser;
                if (user == null)
                {
                    return new ClientLocation() { Result = RequestResponceType.NullParamsError };
                }

                //
                Logger.Trace("GetClientLocationInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                var info = GetClientLocationInfo(param);
                return new ClientLocation() { ClientLocationInfo = info, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new ClientLocation() { Result = RequestResponceType.GlobalError };
            }
        }
        
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("clientInfoApi/getClientInfo", Name = "getClientWorkPlaceInfo")]
        [Obsolete("Удалить, после подтверждения, что никто не использует этот метод")]
        public ClientLocation getClientWorkPlace(ClientIDRequest request)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new ClientLocation() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("GetClientLocationInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                var info = getClientInfo(request);
                return new ClientLocation() { ClientLocationInfo = info, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new ClientLocation() { Result = RequestResponceType.GlobalError };
            }
        }
        
        public sealed class ClientSubDivision
        {
            public СlientSubDivisionInfo СlientSubDivisionInfo { get; set; }
            public RequestResponceType Result { get; set; }
        }
        
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("clientInfoApi/getClientSubDivisionInfo", Name = "getClientSubDivisionInfo")]
        public ClientSubDivision getClientSubDivision(ClientSubDivisionModel model)
        {
            try
            {
                var user = CurrentUser;
                if (user == null)
                {
                    return new ClientSubDivision() { Result = RequestResponceType.NullParamsError };
                }
                //
                Logger.Trace("GetClientLocationInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                var info = getClientSubDivisionInfo(model);
                return new ClientSubDivision() { СlientSubDivisionInfo = info, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new ClientSubDivision() { Result = RequestResponceType.GlobalError };
            }
        }
    }
}