using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Inframanager.BLL.EventsOld;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.Configuration;

namespace InfraManager.BLL.ProductCatalogue.PortTemplatess;

public class PortTemplatesEventBuilder:IConfigureEventBuilder<PortTemplate>
{
    private readonly IFinder<TechnologyType> _technologyTypeFinder;
    private readonly IFinder<ConnectorType> _connectorTypeFinder;
    public void Configure(IBuildEvent<PortTemplate> config)
    {
        config.HasEntityName(nameof(PortTemplate));
        config.HasId(x => x.ObjectID);
        config.HasInstanceName(x => x.PortNumber.ToString());
        config.HasProperty(x => x.PortNumber).HasName("Номер");
        config.HasProperty(x => x.JackTypeID).HasConverter(x=>$"{_connectorTypeFinder.Find(x)?.Name}").HasName("Разъем");
        config.HasProperty(x => x.TechnologyID).HasConverter(x=>$"{_technologyTypeFinder.Find(x)?.Name}").HasName("Технология");
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

    public PortTemplatesEventBuilder(IFinder<TechnologyType> technologyTypeFinder, IFinder<ConnectorType> connectorTypeFinder)
    {
        _technologyTypeFinder = technologyTypeFinder;
        _connectorTypeFinder = connectorTypeFinder;
    }

    public void WhenInserted(IBuildEventOperation<PortTemplate> insertConfig)
    {
        foreach (var historyConfig in _historyMapping)
        {
            insertConfig.HasOperationIf(x => x.ClassID == (int)historyConfig.Key,
                historyConfig.Value.Item1, PortTemplates => $"Добавлен [Порт] '{PortTemplates.PortNumber}");
        }

        insertConfig.HasOperation(OperationID.PortTemplate_Insert, PortTemplates => $"Добавлен [Порт] '{PortTemplates.PortNumber}");

    }

    public void WhenUpdated(IBuildEventOperation<PortTemplate> updateConfig)
    {
        foreach (var historyConfig in _historyMapping)
        {
            updateConfig.HasOperationIf(x => x.ClassID == (int)historyConfig.Key,
                historyConfig.Value.Item2, PortTemplates => $"Изменен [Порт] '{PortTemplates.PortNumber}");
        }
        updateConfig.HasOperation(OperationID.PortTemplate_Update, PortTemplates => $"Изменен [Порт] '{PortTemplates.PortNumber}");
    }

    public void WhenDeleted(IBuildEventOperation<PortTemplate> deleteConfig)
    {
        foreach (var historyConfig in _historyMapping)
        {
            deleteConfig.HasOperationIf(x => x.ClassID == (int) historyConfig.Key,
                historyConfig.Value.Item3, PortTemplates => $"Удален [Порт] '{PortTemplates.PortNumber}");
        }
        deleteConfig.HasOperation(OperationID.PortTemplate_Delete, PortTemplates => $"Удален [Порт] '{PortTemplates.PortNumber}");
    }
}