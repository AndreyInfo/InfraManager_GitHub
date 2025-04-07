using System;
using System.Collections.Generic;

namespace InfraManager.BLL.Settings.TableFilters.TreeSettingsBuilders
{
    public class OrganizationNoTozTreeSettingsBuilder : IBuildTreeSettings
    {
        public TreeSettings Build(IEnumerable<string> searcherParams)
        {
            return new TreeSettings
            {
                TreeType = NavigatorTypes.OrgStructure,
                TargetClassID = new[] { ObjectClass.Organizaton },
                UseAccessIsGranted = false,
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
