using InfraManager.BLL.Search;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    public class AccomplisherUserTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.OrgStructure,
                TargetClassID = new[] { ObjectClass.User },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
                {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Division,
                    ObjectClass.User
                },
                FinishClassID = new[] { ObjectClass.User },
                OperationsID = AccomplisherUserSearcher.Operations
            };
        }
    }
}
