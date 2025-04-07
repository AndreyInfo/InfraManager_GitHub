using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Software;

[ObjectClassMapping(ObjectClass.SoftwareModel)]
[OperationIdMapping(ObjectAction.Insert, OperationID.SoftwareModel_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.SoftwareModel_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.SoftwareModel_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.SoftwareModel_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.SoftwareModel_Properties)]
public class SoftwareModel : Catalog<Guid>, IMarkableForDelete
{
    public SoftwareModel()
    {
        ID = Guid.NewGuid();
        SoftwareTypeID = Guid.Parse("00000000-0000-0000-0000-000000000000");
        SoftwareModelUsingTypeID = Guid.Parse("00000000-0000-0000-0000-000000000000");
        ParentID = null;
        TrueID = null;
        CreateDate = DateTime.UtcNow;
        IsCommercial = false;
        CommercialModelID = null;
        ProcessNames = "";
        ExternalID = "";
        UtcDateCreated = DateTime.UtcNow;
    }

    /// <summary>
    /// Ссылка на тип ПО
    /// </summary>
    public Guid SoftwareTypeID { get; set; }

    /// <summary>
    /// Название модели ПО
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание модели ПО
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// Версия модели ПО
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// Код модели ПО
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Идентификатор производителя модели ПО
    /// </summary>
    public Guid? ManufacturerID { get; set; }

    /// <summary>
    /// Дата до которой производится поддержка данной модели ПО
    /// </summary>
    public DateTime? SupportDate { get; set; }

    /// <summary>
    /// Шаблон модели ПО (справочник)
    /// </summary>
    public SoftwareModelTemplate Template { get; set; }

    /// <summary>
    /// Флаг удаления
    /// </summary>
    public bool Removed { get; set; }

    /// <summary>
    /// Ссылка на родительскую модель ПО
    /// </summary>
    public Guid? ParentID { get; set; }

    /// <summary>
    /// Ссылка на модель ПО, являющуюся синонимом данной
    /// </summary>
    public Guid? TrueID { get; init; }

    /// <summary>
    /// Дата создания модели ПО
    /// </summary>
    public DateTime CreateDate { get; init; }

    /// <summary>
    /// Идентификатор Типа использования
    /// </summary>
    public Guid SoftwareModelUsingTypeID { get; set; }

    /// <summary>
    /// Флаг, является ли модель ПО коммерческой
    /// </summary>
    public bool IsCommercial { get; init; }

    /// <summary>
    /// Ссылка на коммерческую модель ПО
    /// </summary>
    public Guid? CommercialModelID { get; init; }

    /// <summary>
    /// Процессы
    /// </summary>
    public string ProcessNames { get; set; }

    /// <summary>
    /// Внешний идентификатор
    /// </summary>
    public string ExternalID { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime UtcDateCreated { get; init; }

    /// <summary>
    /// Редакция
    /// </summary>
    public string? ModelRedaction { get; set; }

    /// <summary>
    /// Идентификатор Владельца модели
    /// </summary>
    public Guid? OwnerModelID { get; set; }

    /// <summary>
    /// Идентификатор типа Владельца
    /// </summary>
    public int? OwnerModelClassID { get; set; }

    /// <summary>
    /// Дистрибутив
    /// </summary>
    public string? ModelDistribution { get; set; }

    /// <summary>
    /// Процент
    /// </summary>
    public int? PercentComponent { get; set; }


    public Guid? ComplementaryID { get; set; }
    public byte[] RowVersion { get; set; }

    public virtual SoftwareModel CommercialModel { get; }
    public virtual SoftwareModel Parent { get; }
    public virtual SoftwareModelUsingType SoftwareModelUsingType { get; }
    public virtual SoftwareType SoftwareType { get; }
    public virtual SoftwareModel True { get; }
    public virtual LicenseModelAdditionFields LicenseModelAdditionFields { get; }
    public virtual SoftwareModelRecognition SoftwareModelRecognition { get; }

    public virtual ICollection<SoftwareModel> InverseCommercialModel { get; }
    public virtual ICollection<SoftwareModel> InverseParent { get; }
    public virtual ICollection<SoftwareModel> InverseTrue { get; }
    public virtual ICollection<SoftwareInstallation> SoftwareInstallation { get; }
    public virtual ICollection<SoftwareLicence> SoftwareLicence { get; }

    public void MarkForDelete()
    {
        Removed = true;
    }
}
