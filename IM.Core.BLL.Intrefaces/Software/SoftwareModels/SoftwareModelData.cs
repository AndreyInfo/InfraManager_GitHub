using InfraManager.DAL.Software;
using System;

namespace InfraManager.BLL.Software.SoftwareModels;

public class SoftwareModelData
{
    /// <summary>
    /// Ссылка на тип ПО
    /// </summary>
    public Guid SoftwareTypeID { get; init; }

    /// <summary>
    /// Название модели ПО
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Описание модели ПО
    /// </summary>
    public string Note { get; init; }

    /// <summary>
    /// Версия модели ПО
    /// </summary>
    public string Version { get; init; }

    /// <summary>
    /// Код модели ПО
    /// </summary>
    public string Code { get; init; }

    /// <summary>
    /// Идентификатор производителя модели ПО
    /// </summary>
    public Guid ManufacturerID { get; init; }

    /// <summary>
    /// Дата до которой производится поддержка данной модели ПО
    /// </summary>
    public DateTime? SupportDate { get; init; }

    /// <summary>
    /// Шаблон модели ПО (справочник)
    /// </summary>
    public SoftwareModelTemplate Template { get; init; }

    /// <summary>
    /// Флаг удаления
    /// </summary>
    public bool Removed { get; init; }

    /// <summary>
    /// Ссылка на родительскую модель ПО
    /// </summary>
    public Guid? ParentID { get; init; }

    /// <summary>
    /// Ссылка на модель ПО, являющуюся синонимом данной
    /// </summary>
    public Guid? TrueID { get; init; }

    /// <summary>
    /// Идентификатор Типа использования
    /// </summary>
    public Guid? SoftwareModelUsingTypeID { get; init; }

    /// <summary>
    /// Флаг, является ли модель ПО коммерческой
    /// </summary>
    public bool IsCommercial { get; init; }

    /// <summary>
    /// Ссылка на коммерческую модель ПО
    /// </summary>
    public Guid? CommercialModelID { get; init; }

    /// <summary>
    /// Внешний идентификатор
    /// </summary>
    public string ExternalID { get; init; }

    /// <summary>
    /// Редакция
    /// </summary>
    public string? ModelRedaction { get; init; }

    /// <summary>
    /// Идентификатор Владельца модели
    /// </summary>
    public Guid? OwnerModelID { get; init; }

    /// <summary>
    /// Идентификатор типа Владельца
    /// </summary>
    public int? OwnerModelClassID { get; init; }

    /// <summary>
    /// Дистрибутив
    /// </summary>
    public string? ModelDistribution { get; init; }

    /// <summary>
    /// Процент
    /// </summary>
    public int? PercentComponent { get; init; }

    public SoftwareModelLanguage? LanguageID { get; init; }
    public Guid? LicenseSchemeID { get; init; }
    public Guid? GroupQueueID { get; init; }
    public int? VersionRecognitionID { get; init; }
    public int? VersionRecognitionLvl { get; init; }
    public int? RedactionRecognition { get; init; }

    public Guid? ComplementaryID { get; init; }
}
