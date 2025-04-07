using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    internal class SubDivisionTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.OrgStructure,
                TargetClassID = new[] { ObjectClass.Division },
                UseAccessIsGranted = true,
                AvailableClassID = new[]
                {
                    ObjectClass.Organizaton,
                    ObjectClass.Division,
                    ObjectClass.User
                },
                FinishClassID = Array.Empty<ObjectClass>(),
                OperationsID = Array.Empty<OperationID>()
            };
        }
    }
}
