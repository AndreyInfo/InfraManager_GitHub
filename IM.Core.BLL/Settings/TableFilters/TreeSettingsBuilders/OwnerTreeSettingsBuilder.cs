using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    public class OwnerTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.OrgStructure,
                TargetClassID = new[] { ObjectClass.Organizaton },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
                {
                    ObjectClass.Organizaton,
                    ObjectClass.Owner
                },
                FinishClassID = new[] { ObjectClass.Organizaton },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
