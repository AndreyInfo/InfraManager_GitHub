using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class RackTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.Location,
                TargetClassID = new [] 
                {
                    ObjectClass.Rack 
                },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
               {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Building,
                    ObjectClass.Floor,
                    ObjectClass.Room,
                    ObjectClass.Rack
                },
                FinishClassID = new[]
                {
                    ObjectClass.Rack
                },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
