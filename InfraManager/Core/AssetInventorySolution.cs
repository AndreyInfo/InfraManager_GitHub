using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core
{
    public enum AssetInventorySolution : byte
    {
        [ResourcesArea.Display(nameof(ResourcesArea.Resources.InventorySpecification_Confirmed))]
        [FriendlyName("Подтверждено")]
        Confirmed = 0,
        [ResourcesArea.Display(nameof(ResourcesArea.Resources.InventorySpecification_OperationExecuted))]
        [FriendlyName("Выполнена операция")]
        OperaionExecuted = 1,
        [ResourcesArea.Display(nameof(ResourcesArea.Resources.InventorySpecification_DB_Updated))]
        [FriendlyName("База изменена")]
        DB_Updated = 2,
        [ResourcesArea.Display(nameof(ResourcesArea.Resources.InventorySpecification_Ignored))]
        [FriendlyName("Игнорировать")]
        Ignore = 3,
    }

    public enum UICommand : byte
    {
        [FriendlyName("Включить")]
        InvInclude = 0,

        [FriendlyName("Добавить строку инвентаризации")]
        InvAdd = 1,

        [FriendlyName("Выгрузить в отчет")]
        InvToReport = 2,

        [FriendlyName("Инвентаризация. Редактировать исходные данные")]
        InvEditSource = 3,

        [FriendlyName("Редактировать результаты инвентаризации")]
        InvEditResults = 4,

        [FriendlyName("Удалить строку инвентаризации")]
        InvDelete = 5,

        [FriendlyName("Инвентаризация. Решение. Подтвердить")]
        InvSolutionConfirm = 6,

        [FriendlyName("Инвентаризацияю. Решение. Игнорировать")]
        InvSolutionIgnore = 7,

        [FriendlyName("Инвентаризацияю. Решение. Выполнить операцию")]
        InvSolutionRunAction = 8,

        [FriendlyName("Инвентаризацияю. Решение. Изменить базу")]
        InvSolutionUpdate = 9,

        [FriendlyName("Инвентаризацияю. Решение. Новый объект")]
        InvSolutionNewActive = 10,
    }
}
