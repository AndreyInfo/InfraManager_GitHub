using System;
using Inframanager;

namespace InfraManager.DAL.Dashboards;

[ObjectClassMapping(ObjectClass.DashboardItem)]
[OperationIdMapping(ObjectAction.Delete, OperationID.DashboardItem_Delete)]
[OperationIdMapping(ObjectAction.Insert, OperationID.DashboardItem_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.DashboardItem_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.DashboardItem_Properties)]
public class DashboardItem
{
    public Guid ID { get; init; }
    
    public string Name { get; set; }
    
    public Guid DashboardID { get; set; }
    
    public virtual Dashboard Dashboard { get; }
    
    public decimal Left { get; set; }
    
    public decimal Top { get; set; }
    
    public int ZIndex { get; set; }
    
    public decimal Width { get; set; }
    
    public decimal Height { get; set; }
    
    public int BackgroundColor { get; set; }
    
    public int TextColor { get; set; }
    
    public WidgetType WidgetType { get; set; }
    
    public string FactorType  { get; set; }
    
    public byte[] FactorData  { get; set; }
}