using System;
using System.Collections.Generic;
using System.Linq;
using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.Web.BLL.Navigator;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.Web.Controllers.Utility
{
    public class NavigatorApiController : BaseApiController
    {

        public sealed class TreeNodesIncomingModel
        {
            public Guid? ID { get; set; }
            public int? ClassID { get; set; }
            public NavigatorTypes Type { get; set; }
            public List<int> AvailableClassID { get; set; }
            public List<int> OperationsID { get; set; }
            public bool UseAccessIsGranted { get; set; }
            public Guid? AvailableCategoryID { get; set; }
            public List<int> RemovedCategoryClassArray { get; set; }
            public bool? UseRemoveCategoryClass { get; set; }
            public Guid? AvailableTypeID { get; set; }
            public int? AvailableTemplateClassID { get; set; }
            public int? AvailableTemplateID { get; set; }
            public List<int> AvailableTemplateClassArray { get; set; }
            public bool HasLifeCycle { get; set; }
            //
            public Guid? CustomControlObjectID { get; set; }
            public bool? SetCustomControl { get; set; }
            public bool? SetTransferOwner { get; set; }
            public bool AllModel { get; set; } = false;

        }
        public sealed class TreeNodesOutModel
        {
            public IList<TreeNode> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        #region method GetTreeNodes
        [HttpPost]
        [Route("navigatorApi/GetTreeNodes", Name = "GetTreeNodes")]
        public TreeNodesOutModel GetTreeNodes([FromBodyOrForm]TreeNodesIncomingModel model)
        {
            var user = base.CurrentUser;
            if (user == null || model == null)
                return new TreeNodesOutModel() { List = null, Result = RequestResponceType.NullParamsError }; ;

            Logger.Trace("NavigatorApiController.GetTreeNodes userID={0}, userName={1}, navigatorType={2}", user.Id, user.UserName, model.Type);

            List<TreeNode> retval;
            //
            if ((NavigatorTypes)Enum.Parse(typeof(NavigatorTypes), model.Type.ToString()) == NavigatorTypes.None)
                return new TreeNodesOutModel() { List = null, Result = RequestResponceType.Success };
            //
            using (var dataSource = DataSource.GetDataSource())
            {
                try
                {
                    var mem = typeof(NavigatorTypes).GetMember(model.Type.ToString()).FirstOrDefault();
                    var typeObj = (Attribute.GetCustomAttributes(mem, typeof(NavigatorClassAttribute)).FirstOrDefault() as NavigatorClassAttribute).Type;
                    //
                    if (model.ID.HasValue && model.ClassID.HasValue)
                    {
                        if (model.UseRemoveCategoryClass != null)//работа с исключаениями сделана только для ProductCatalog
                        {
                            var method = typeObj.GetMethod(nameof(BaseNavigator.GetNextListWithException));
                            var classObj = Activator.CreateInstance(typeObj);
                            retval = method.Invoke(classObj, new object[] { model.ID.Value, model.ClassID.Value, model.UseAccessIsGranted ? user.User.ID : (Guid?)null, model.AvailableClassID, model.OperationsID, model.AvailableCategoryID, model.AvailableTypeID, model.AvailableTemplateClassID, model.AvailableTemplateClassArray, model.AvailableTemplateID, model.CustomControlObjectID, model.SetCustomControl, model.SetTransferOwner, model.HasLifeCycle, model.UseRemoveCategoryClass, model.RemovedCategoryClassArray, dataSource }) as List<TreeNode>;
                        }
                        else
                        {
                            var method = typeObj.GetMethod(nameof(BaseNavigator.GetNextList));
                            var classObj = Activator.CreateInstance(typeObj);
                            retval = method.Invoke(classObj, new object[] { model.ID.Value, model.ClassID.Value, model.UseAccessIsGranted ? user.User.ID : (Guid?)null, model.AvailableClassID, model.OperationsID, model.AvailableCategoryID, model.AvailableTypeID, model.AvailableTemplateClassID, model.AvailableTemplateClassArray, model.AvailableTemplateID, model.CustomControlObjectID, model.SetCustomControl, model.SetTransferOwner, model.HasLifeCycle, model.AllModel, dataSource }) as List<TreeNode>;
                        }
                    }
                    else
                    {
                        var method = typeObj.GetMethod(nameof(BaseNavigator.GetStartedList));
                        var classObj = Activator.CreateInstance(typeObj);
                        retval = method.Invoke(classObj, new object[] { dataSource }) as List<TreeNode>;
                    }

                    List<TreeNode> sortedRetVal = retval.OrderBy(tn => tn.Name).ToList();
                    return new TreeNodesOutModel() 
                    {
                        List = sortedRetVal, 
                        Result = RequestResponceType.Success 
                    };
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка получения списка узлов дерева.");
                    return new TreeNodesOutModel()
                    { 
                        List = null, 
                        Result = RequestResponceType.GlobalError 
                    };
                }
            }
        }
        #endregion

        #region method GetPathToNode
        [HttpGet]
        [Route("navigatorApi/GetPathToNode", Name = "GetPathToNode")]
        public TreeNodesOutModel GetPathToNode(Guid id, int classID, NavigatorTypes type)
        {
            var user = base.CurrentUser;
            if (user == null)
                return new TreeNodesOutModel() { List = null, Result = RequestResponceType.NullParamsError }; 
            else if(type == NavigatorTypes.None)
                return new TreeNodesOutModel() { List = null, Result = RequestResponceType.Success }; 
            //
            Logger.Trace("NavigatorApiController.GetPathToNode userID={0}, userName={1}, navigatorType={2}", user.Id, user.UserName, type);
            //
            List<TreeNode> retval;
            //
            using (var dataSource = DataSource.GetDataSource())
            {
                try
                {
                    var mem = typeof(NavigatorTypes).GetMember(type.ToString()).FirstOrDefault();                    
                    var typeObj = (Attribute.GetCustomAttributes(mem, typeof(NavigatorClassAttribute)).FirstOrDefault() as NavigatorClassAttribute).Type;
                    //
                    var method = typeObj.GetMethod(nameof(BaseNavigator.GetPathToNode));
                    var obj = Activator.CreateInstance(typeObj);
                    retval = method.Invoke(obj, new object[] { id, classID, dataSource }) as List<TreeNode>;
                    //
                    return new TreeNodesOutModel() { List = retval, Result = RequestResponceType.Success }; 
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка получения списка пути узлов дерева.");
                    return new TreeNodesOutModel() { List = null, Result = RequestResponceType.GlobalError }; 
                }
            }
        }
        #endregion

        #region method GetTreeNodeIconClass
        [HttpGet]
        [Route("navigatorApi/GetTreeNodeIconClass", Name = "GetTreeNodeIconClass")]
        public object GetTreeNodeIconClass(int classID)
        {
            return new { classId = TreeNode.GetIcon(classID) };
        }
        #endregion
    }
}
