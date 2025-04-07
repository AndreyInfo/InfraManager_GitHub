using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class WorkplaceTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.Location,
                TargetClassID = new [] 
                {
                    ObjectClass.Workplace,
                },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
               {
                    ObjectClass.Owner,
                    ObjectClass.Organizaton,
                    ObjectClass.Building,
                    ObjectClass.Floor,
                    ObjectClass.Room,
                    ObjectClass.Workplace
                },
                FinishClassID = new[]
                {
                    ObjectClass.Workplace
                },
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
