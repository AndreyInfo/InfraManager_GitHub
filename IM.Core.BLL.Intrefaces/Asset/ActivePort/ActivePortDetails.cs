﻿using System;

namespace InfraManager.BLL.Asset.ActivePort;

public class ActivePortDetails
{
    public int ID { get; init; }
    public string PortName { get; init; }
    public int? JackTypeID { get; init; }
    public string JackTypeName { get; init; }
    public int? TechnologyTypeID { get; init; }
    public string TechnologyTypeName { get; init; }
    public string PortAddress { get; init; }
    public string PortIPX { get; init; }
    public string GroupNumber { get; init; }
    public string PortSpeed { get; init; }
    public int? PortVLAN { get; init; }
    public string PortFilter { get; init; }
    public int? PortState { get; init; }
    public int PortStatus { get; init; }
    public Guid? PortModule { get; init; }
    public int? SlotNumber { get; init; }
    public string Description { get; init; }
    public string Note { get; init; }
    public int? ActiveEquipmentID { get; init; }
    public int? PortNumber { get; init; }
}

