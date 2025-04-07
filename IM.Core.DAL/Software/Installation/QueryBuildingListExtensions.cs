using System;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Software.Installation
{
    internal static class QueryBuildingListExtensions
    {
        /// <summary>
        /// Формирует уникальный список Guid для списка из QueryBuildingListItem
        /// </summary>
        /// <param name="queryBuildingListItems"> список Guid в виде трех значений </param>
        /// <returns> список Guid </returns>
        public static IEnumerable<Guid> QueryBuildingListItemToListGuid(this IEnumerable<QueryBuildingListItem> queryBuildingListItems)
        {
            var guidList = new List<Guid>();

            foreach (var item in queryBuildingListItems)
            {
                if (item.ActiveDeviceImobjId.HasValue)
                {
                    guidList.Add(item.ActiveDeviceImobjId.Value);
                }

                if (item.TerminalEquipment1ImobjId.HasValue)
                {
                    guidList.Add(item.TerminalEquipment1ImobjId.Value);
                }

                if (item.TerminalEquipment2ImobjId.HasValue)
                {
                    guidList.Add(item.TerminalEquipment2ImobjId.Value);
                }
            }

            return guidList.Distinct();
        }

    }
}
