using IM.Core.WF.BLL.Interfaces.Models;
using System;
using System.Collections.Generic;

namespace IM.Core.WF.BLL.Interfaces
{
    public interface IWorkflowShemeBLL
    {
        WorkflowSchemeModel[] GetActiveSchemes();

        WorkflowSchemeModel Get(Guid id);

        WorkflowSchemeModel[] GetAllByParent(Guid? workflowSchemeFolderID);

        string GetVisualScheme(Guid id);
        
        string GetLogicalScheme(Guid id);

        WorkflowSchemeModel GetPublishedScheme(string id);

        (ushort majorV, ushort minorV) GetMaxVersion(string identifier);

        List<Tuple<Guid, string, string, byte, string, string>> GetCustomFilterSchemes(Guid userID, int objectClassID);

        List<Guid> GetBlockedSchemes();

        List<Tuple<Guid, string, string, string>> GetLogicalSchemesForExistsObjects();

        WorkflowSchemeModel[] GetList();

        WorkflowSchemeModel[] GetListByIdentifier(string identifier);
        
        bool ExistsByID(Guid id);

        bool ExistsByName(Guid id, string name, string identifier, Guid? workflowSchemeFolderID);
        
        bool ExistsByIdentifier(string identifier);

        bool InUse(Guid id);
        
        WorkflowSchemeModel[] GetList(int skip, int take, string searchString, int? classID, byte? status);
        bool Delete(Guid id, byte[] rowVersion);
        void Insert(WorkflowSchemeModel model);
        void Save(WorkflowSchemeModel model);
    }
}
