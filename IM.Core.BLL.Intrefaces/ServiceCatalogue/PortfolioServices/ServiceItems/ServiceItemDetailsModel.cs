using System;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceItems;

public class ServiceItemDetailsModel // TODO: Model убрать (пока мешает дубль BLL, который сначала надо будет выпилить)
{
    public Guid ID { get; init; }
    public ObjectClass ClassID { get; init; }
    public Guid ServiceCategoryID { get; init; }
    public string ServiceCategoryName { get; init; }
    public Guid ServiceID { get; init; }
    public string ServiceName { get; init; }
    public Guid? ServiceItemID => ClassID == ObjectClass.ServiceItem ? ID : null;
    public Guid? ServiceAttendanceID => ClassID == ObjectClass.ServiceAttendance ? ID : null;
    public string FullName { get; init; }
    public string Name { get; init; }
    public string Note { get; set; }
    public string Parameter { get; init; }
    public string Summary { get; init; }
    public bool IsInFavorite { get; set; }
    public bool IsAvailable { get; set; }
    public Guid? FormID { get; init; }
}
