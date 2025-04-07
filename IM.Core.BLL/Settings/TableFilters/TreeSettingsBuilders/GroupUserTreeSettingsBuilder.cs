using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class GroupUserTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.OrgStructure,
                TargetClassID = new[] { ObjectClass.User, ObjectClass.Group },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
                {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Division,
                    ObjectClass.User
                },
                FinishClassID = new[] { ObjectClass.User },
                OperationsID = new []
                {
                    OperationID.SD_General_Administrator,
                    OperationID.SD_General_Executor
                }
            };
        }
    }
}
