using InfraManager.Core.Logging;
using InfraManager.Web.BLL.ObjectSearchers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static InfraManager.Web.Scripts.models.ClientSearch.ClientPosition;
using Microsoft.AspNetCore.Mvc;
using InfraManager.ComponentModel;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.BLL.Users;

namespace InfraManager.Web.Controllers.Utility
{
    public class SearchApiController : BaseApiController
    {
        private readonly IUserBLL _users;

        public SearchApiController(IUserBLL users)
        {
            _users = users;
        }

        public class stringResult
        {
            public string result { get; set; }
        } 

        #region method Search
        [HttpPost]
        [Route("searchApi/search", Name = "MainSearch")]
        public ActionResult<IObject[]> Search([FromBodyOrForm]SearchRequest data)
        {
            if (string.IsNullOrWhiteSpace(data.Text))
                data.Text = string.Empty;
            //
            data.Text = System.Web.HttpUtility.UrlDecode(data.Text);
            data.Text = data.Text.Replace(@"%", @"[%]");
            //
            Guid? targetUserID = data.CurrentUserID.HasValue ? (Guid?)data.CurrentUserID.Value : null; //для подмены в тех искалках, где она требуется
            Guid currentUserID = base.CurrentUser.User.ID;
            //
            var connectionID = BLL.Global.GetRequestConnectionID();
            //
            try
            {
                List<string> param = new List<string>();
                JArray jArray = JArray.Parse(data.Params);
                foreach (var item in jArray)
                    param.Add(item.Value<string>());
                    //
                var searcher = ObjectSearcher.Get(data.TypeName, param, currentUserID, targetUserID, connectionID);
                return searcher.Search(data.Text).ToArray();//TODO: all user can access to results
            }
            catch (ArgumentException e)
            {
                Logger.Error("Cannot find or create searcher: {0}", e.Message);
                return BadRequest(e.Message); //TODO: exposing exception is insecure
            }
            catch (Exception e)
            {
                Logger.Error("Search error: {0}", e.Message);
                return Array.Empty<IObject>();
            };
        }
        #endregion

        #region method CallClientServiceLocation
        public sealed class CallClientLocation
        {
            public CallСlientLocationInfo ClientLocationInfo { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("searchApi/GetCallClientLocationInfo", Name = "getCallClientLocationInfo")]
        public CallClientLocation GetCallClientLocation([FromQuery] ClientLocationModel param)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return new CallClientLocation() { Result = RequestResponceType.NullParamsError };
                //
                Logger.Trace("GetCallClientLocationInfo userID={0}, userName={1}", user.Id, user.UserName);
                //
                CallСlientLocationInfo info = GetCallClientLocationInfo(param);
                return new CallClientLocation() { ClientLocationInfo = info, Result = RequestResponceType.Success };
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new CallClientLocation() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetObjectFullName
        [HttpGet]
        [Route("searchApi/getObjectFullName", Name = "GetObjectFullName")]
        public stringResult GetObjectFullName([FromQuery] Guid objectID, [FromQuery] int objectClassID)
        {
            stringResult result = new stringResult();
            var currentUser = base.CurrentUser;
            if (currentUser == null)
                return null;
            //
            string retval;
            switch (objectClassID)
            {//эти исключения используются для параметров заявки/задания (тип параметра-местоположение)
                case IMSystem.Global.OBJ_BUILDING:
                case IMSystem.Global.OBJ_FLOOR:
                case IMSystem.Global.OBJ_ROOM:
                case IMSystem.Global.OBJ_WORKPLACE:
                case IMSystem.Global.OBJ_RACK:
                    retval = ObjectSearcher.GetObjectFullNameForLocation(objectID, objectClassID);//TODO: all user can access to results
                    break;
                case IMSystem.Global.OBJ_ParameterEnum:
                    retval = BLL.Parameters.ParameterValue.GetParameterValueName(objectID);
                    break;
                default:
                    retval = ObjectSearcher.GetObjectFullName(objectID, objectClassID);//TODO: all user can access to results
                    break;
            }
            result.result = retval;
            return result;
        }
        #endregion

        #region helper class SearchRequest
        public sealed class SearchRequest
        {
            #region SearchRequest
            public SearchRequest()
            { }
            #endregion

            #region properties
            public string Text { get; set; }
            public string TypeName { get; set; }
            public string Params { get; set; }
            public Guid? CurrentUserID { get; set; }
            #endregion
        }
        #endregion
    }
}
