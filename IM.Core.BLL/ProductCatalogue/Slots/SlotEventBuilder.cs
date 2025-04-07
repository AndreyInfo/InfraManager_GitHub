using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Slots;

public class SlotEventBuilder:IConfigureEventBuilder<Slot>
{
    private readonly IFinder<SlotType> _finder;
    public void Configure(IBuildEvent<Slot> config)
    {
        config.HasEntityName(nameof(Slot));
        config.HasId(x => x.ObjectID);
        config.HasInstanceName(x => x.Number.ToString());
        config.HasProperty(x => x.Number).HasName("Номер");
        config.HasProperty(x => x.SlotTypeID).HasConverter(x=>$"{_finder.Find(x)?.Name}").HasName("Тип");
    }

    private readonly Dictionary<ObjectClass, (OperationID, OperationID, OperationID)> _historyMapping = new()
    {
        {
            ObjectClass.AdapterModel,
            (OperationID.AdapterModel_Add, OperationID.AdapterModel_Update, OperationID.AdapterModel_Delete)
        },
        {
            ObjectClass.NetworkDeviceModel,
            (OperationID.NetworkDeviceModel_Add, OperationID.NetworkDeviceModel_Update,
                OperationID.NetworkDeviceModel_Delete)
        },
        {
            ObjectClass.PeripherialModel,
            (OperationID.PeripheralModel_Add, OperationID.PeripheralModel_Update, OperationID.PeripheralModel_Delete)
        },
        {
            ObjectClass.TerminalDeviceModel,
            (OperationID.TerminalDeviceModel_Add, OperationID.TerminalDeviceModel_Update,
                OperationID.TerminalDeviceModel_Delete)
        }
    };

    public SlotEventBuilder(IFinder<SlotType> finder)
    {
        _finder = finder;
    }

    public void WhenInserted(IBuildEventOperation<Slot> insertConfig)
    {
        foreach (var historyConfig in _historyMapping)
        {
            insertConfig.HasOperationIf(x => x.ObjectClassID == (int)historyConfig.Key,
                historyConfig.Value.Item1, slot => $"Добавлен [Слот] '{slot.Number}");
        }

        insertConfig.HasOperation(OperationID.Slot_Add, slot => $"Добавлен [Слот] '{slot.Number}");

    }

    public void WhenUpdated(IBuildEventOperation<Slot> updateConfig)
    {
        foreach (var historyConfig in _historyMapping)
        {
            updateConfig.HasOperationIf(x => x.ObjectClassID == (int)historyConfig.Key,
                historyConfig.Value.Item2, slot => $"Изменен [Слот] '{slot.Number}");
        }
        updateConfig.HasOperation(OperationID.Slot_Update, slot => $"Изменен [Слот] '{slot.Number}");
    }

    public void WhenDeleted(IBuildEventOperation<Slot> deleteConfig)
    {
        foreach (var historyConfig in _historyMapping)
        {
            deleteConfig.HasOperationIf(x => x.ObjectClassID == (int) historyConfig.Key,
                historyConfig.Value.Item3, slot => $"Удален [Слот] '{slot.Number}");
        }
        deleteConfig.HasOperation(OperationID.Slot_Delete, slot => $"Удален [Слот] '{slot.Number}");
    }
}