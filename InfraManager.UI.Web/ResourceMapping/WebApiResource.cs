namespace InfraManager.UI.Web.ResourceMapping
{
    /// <summary>
    /// Это перечисление сопоставляет названия ресурсов (в REST стиле) из идентификаторов классов сущностей
    /// Константа должна называться так же как контроллер соответствующей сущности, а соответствующее ей числовое значение должно быть равно
    /// classID из ObjectClass
    /// Константы должны перечисляться в той же последовательности, что и в ObjectClass (т.е. по возрастанию classID).
    /// </summary>
    public enum WebApiResource : int
    {
        WorkOrders = ObjectClass.WorkOrder,
        ServiceItems = ObjectClass.ServiceItem,
        ServiceAttendances = ObjectClass.ServiceAttendance,
        Calls = ObjectClass.Call,       
        Problems = ObjectClass.Problem,
        ChangeRequests = ObjectClass.ChangeRequest,
        MassIncidents = ObjectClass.MassIncident,
        MassIncidentTypes = ObjectClass.MassIncidentType
    }
}
