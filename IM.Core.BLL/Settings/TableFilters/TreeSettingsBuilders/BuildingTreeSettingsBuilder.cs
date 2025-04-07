using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class BuildingTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.Location,
                TargetClassID = new[] { ObjectClass.Building },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
               {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Building
                },
                FinishClassID = new[]
                {
                    ObjectClass.Building
                },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
