using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Snmp
{
    /// <summary>
    /// Фильтр для поиска объектов «Правило распознавания» для внутрених сервисов
    /// </summary>
    public sealed class SnmpDeviceModelFilter : BaseFilter
    {
        /// <summary>
        /// Фильтрация по каталогу продуктов ('Типы активного оборудования'/'active_equipment_types')
        /// </summary>
        public int[] ModelIDs { get; init; }

        /// <summary>
        /// Фильтрация по значению поля «Ответ по sysObjectID»
        /// </summary>
        public string SysObjectIDValue { get; init; }

        /// <summary>
        /// Фильтрация по значению поля «Тэг из sysDescr»
        /// </summary>
        public string DescriptionTag { get; init; }

        /// <summary>
        /// Фильтрация по значению поля «Ответ по OID»
        /// </summary>
        public string OIDValue { get; init; }
    }
}
