using InfraManager.BLL.Settings.TableFilters.Attributes;
using Inframanager.BLL;
using InfraManager.ResourcesArea;
using System;

namespace InfraManager.BLL.OrganizationStructure
{
    public abstract class BaseDeputyUserListItem
    {
        public Guid ID { get; init; }

        public Guid ParentUserID { get; init; }

        public Guid ChildUserID { get; init; }

        
        public abstract string UserFullName { get; init; }

        // TODO: должно стать UtcDeputySince в будущем
        [LikeFilter]
        [ColumnSettings(1)]
        [Label(nameof(Resources.With))]
        public DateTime UtcDataDeputyWith { get; init; }

        // TODO: должно стать UtcDeputyUntil в будущем
        [LikeFilter]
        [ColumnSettings(2)]
        [Label(nameof(Resources.By))]
        public DateTime UtcDataDeputyBy { get; init; }


        public string UtcDataDeputyBySt { get; init; }

        public string UtcDataDeputyWithSt { get; init; }
    }
}
