using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class RoomTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.Location,
                TargetClassID = new [] 
                {
                    ObjectClass.Room
                },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
               {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Building,
                    ObjectClass.Floor,
                    ObjectClass.Room
                },
                FinishClassID = new[]
                {
                    ObjectClass.Room
                },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
