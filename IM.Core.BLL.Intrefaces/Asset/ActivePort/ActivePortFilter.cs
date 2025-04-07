﻿using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Asset.ActivePort;

public class ActivePortFilter : BaseFilter
{
    public int? ActiveEquipmentID { get; init; }
    public string PortName { get; init; }
}