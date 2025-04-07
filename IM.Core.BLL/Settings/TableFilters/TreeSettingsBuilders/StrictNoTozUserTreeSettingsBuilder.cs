using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class StrictNoTozUserTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.OrgStructure,
                TargetClassID = new[] { ObjectClass.User },
                UseAccessIsGranted = false,
                AvailableClassID = new[]
                {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Division,
                    ObjectClass.User
                },
                FinishClassID = new[] { ObjectClass.User },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
