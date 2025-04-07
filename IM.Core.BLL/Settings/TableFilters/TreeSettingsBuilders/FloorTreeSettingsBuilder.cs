using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class FloorTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.Location,
                TargetClassID = new [] 
                {
                    ObjectClass.Floor
                },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
               {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Building,
                    ObjectClass.Floor
                },
                FinishClassID = new[]
                {
                    ObjectClass.Floor
                },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
