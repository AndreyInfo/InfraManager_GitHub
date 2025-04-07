using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class NoneTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                AvailableClassID = Array.Empty<ObjectClass>(), 
                FinishClassID = Array.Empty<ObjectClass>(),
                OperationsID = Array.Empty<OperationID>(),
                TargetClassID = Array.Empty<ObjectClass>(),
                UseAccessIsGranted = false,
                TreeType = NavigatorTypes.None
            };
        }
    }
}
