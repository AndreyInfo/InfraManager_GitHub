using Inframanager;
using System;

namespace InfraManager.DAL.Import.ITAsset;

/// <summary>
/// Этот класс представляет сущность Настройки задания импорта ит-активов
/// </summary>
[ObjectClassMapping(ObjectClass.ITAssetImportSetting)]
[OperationIdMapping(ObjectAction.Delete, OperationID.ITAssetImportSetting_Delete)]
[OperationIdMapping(ObjectAction.Insert, OperationID.ITAssetImportSetting_Add)]
[OperationIdMapping(ObjectAction.InsertAs, OperationID.ITAssetImportSetting_AddAs)]
[OperationIdMapping(ObjectAction.Update, OperationID.ITAssetImportSetting_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.ITAssetImportSetting_Properties)]
[OperationIdMapping(ObjectAction.Execute, OperationID.ITAssetImportSetting_Execute)]
[OperationIdMapping(ObjectAction.Plan, OperationID.ITAssetImportSetting_Plan)]
public class ITAssetImportSetting
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public Guid? ITAssetImportCSVConfigurationID { get; init; }
    public string Path { get; init; }

    public bool? CreateModelAutomatically { get; init; }
    public string DefaultModelID { get; init; }
    public bool? MissingModelInSource { get; init; }
    public bool? MissingTypeInSource { get; init; }
    public bool? UnsuccessfulAttemptToCreateAutomatically { get; init; }
    public bool? AddToWorkplaceOfUser { get; init; }
    public string DefaultLocationID { get; init; }
    public bool DefaultLocationNotSpecifiedID { get; init; }
    public bool DefaultLocationNotFoundID { get; init; }
    public bool? CreateDeviation { get; init; }
    public bool? CreateMessages { get; init; }
    public bool? CreateSummaryMessages { get; init; }
    public string WorkflowID { get; init; }
    public ITAssetIdentificationParameterEnum NetworkAndTerminalIdenParam { get; init; }
    public ITAssetIdentificationParameterEnum AdapterAndPeripheralIdenParam { get; init; }
    public ITAssetIdentificationParameterEnum CUIdenParam { get; init; }
    public byte[] RowVersion { get; init; }

    public virtual ITAssetImportCSVConfiguration ITAssetImportCSVConfiguration { get; init; }
}
