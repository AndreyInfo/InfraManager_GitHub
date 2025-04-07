namespace InfraManager.DAL.ServiceDesk.WorkOrders
{
    public enum WorkOrderTypeClass : byte
    {
        /// <summary>
        ///     Задание
        /// </summary>
        WorkOrder = 0,

        /// <summary>
        ///     Запрос на активы
        /// </summary>
        ActivesRequest = 1,

        /// <summary>
        ///     Закупка
        /// </summary>
        Purchase = 2,

        /// <summary>
        ///     Инвентаризация
        /// </summary>
        Inventorization = 3,

        /// <summary>
        ///     Управление знаниями
        /// </summary>
        KnowledgeBaseManagement = 4
    }
}
