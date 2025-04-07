using System;
using System.Collections.Generic;
using System.Text;

namespace InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models
{
    public class SoftwareModel
    {
        /// <summary>
        /// Идентификатор модели ПО
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ссылка на тип ПО
        /// </summary>
        public Guid SoftwareTypeId { get; set; }

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
        public Guid? ManufacturerId { get; set; }

        /// <summary>
        /// Дата до которой производится поддержка данной модели ПО
        /// </summary>
        public DateTime? SupportDate { get; set; }

        /// <summary>
        /// Шаблон модели ПО (справочник)
        /// </summary>
        public byte Template { get; set; }

        /// <summary>
        /// Флаг удаления
        /// </summary>
        public bool Removed { get; set; }

        /// <summary>
        /// Ссылка на родительскую модель ПО
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Ссылка на модель ПО, являющуюся синонимом данной
        /// </summary>
        public Guid? TrueId { get; set; }

        /// <summary>
        /// Дата создания модели ПО
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Версия строки
        /// </summary>
        public byte[] RowVersion { get; set; }
        public Guid? ComplementaryId { get; set; }
        public Guid SoftwareModelUsingTypeId { get; set; }
        public bool IsCommercial { get; set; }
        public Guid? CommercialModelId { get; set; }
        public string ProcessNames { get; set; }
        public string ExternalId { get; set; }

        public SoftwareModel CommercialModel { get; set; }
        public SoftwareModel Parent { get; set; }
        public SoftwareModelUsingType SoftwareModelUsingType { get; set; }
        public SoftwareType SoftwareType { get; set; }
        public SoftwareModel True { get; set; }

    }
    public class SoftwareModelUsingType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsDefault { get; set; }
        public byte[] RowVersion { get; set; }

    }

    public class SoftwareType
    {
        /// <summary>
        /// Идентификатор типа ПО
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Ссылка на родительский тип ПО
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// Название типа ПО
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание типа ПО
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Версия строки
        /// </summary>
        public byte[] RowVersion { get; set; }
        public Guid? ComplementaryId { get; set; }

        public SoftwareType Parent { get; set; }

    }
}
