using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.Configuration
{
    public class ConfigurationUnit
    {
        public Guid? ID { get; set; }
        public string DataCenter { get; set; }
        public string VCenter { get; set; }
        public Guid? LifeCycleStateID { get; set; }
        public string LifeCycleStateName { get; set; }
        public LifeCycleState LifeCycleState { get; set; }
        public bool IsNetworkDevice { get; set; }
        public NetworkDevice Cluster { get; set; }
        public string ObjectName { get; set; }
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public string IPMask { get; set; }
        public string SnmpSecurityParametersName { get; set; }
        public Guid? TypeID { get; set; }
        public string TypeName { get; set; }
        public ProductCatalogType Type { get; set; }
        public string StateName { get; set; }
        public string CriticalityName { get; set; }
        public string InfrastructureSegmentName { get; set; }
        public string AdministratorName { get; set; }
        public Tuple<int, Guid> Administrator { get; set; }
        public int ObjectClassID { get; set; }
        public Guid? ObjectID { get; set; }
        public Guid? ClusterID { get; set; }
        public string ClusterName { get; set; }
    }
}
