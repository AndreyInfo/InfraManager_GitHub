using Inframanager;
using System;

namespace InfraManager.DAL.Import.ITAsset;

/// <summary>
/// Этот класс представляет сущность Настройки конфигурации импорта ит-активов
/// </summary>
[ObjectClassMapping(ObjectClass.ITAssetImportCSVConfiguration)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ITAssetImportCSVConfiguration_Delete)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ITAssetImportCSVConfiguration_Add)]
[OperationIdMapping(ObjectAction.InsertAs, OperationID.ITAssetImportCSVConfiguration_AddAs)]
[OperationIdMapping(ObjectAction.Update, OperationID.ITAssetImportCSVConfiguration_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ITAssetImportCSVConfiguration_Properties)]
public class ITAssetImportCSVConfiguration
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public string Delimeter { get; init; }
    public byte[] RowVersion { get; init; }
}
