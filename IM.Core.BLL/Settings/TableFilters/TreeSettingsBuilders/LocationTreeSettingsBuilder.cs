using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class LocationTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.Location,
                TargetClassID = new [] 
                {
                    ObjectClass.Building,
                    ObjectClass.Floor,
                    ObjectClass.Room,
                    ObjectClass.Workplace,
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
                    ObjectClass.Workplace,
                    ObjectClass.Rack
                },
                FinishClassID = new[]
                {
                    ObjectClass.Workplace,
                    ObjectClass.Rack
                },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
