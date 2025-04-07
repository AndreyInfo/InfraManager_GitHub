using InfraManager.DAL.Import.CSV;
using System;
using Inframanager.DAL.ActiveDirectory.Import;


namespace InfraManager.DAL.Import
{
    public class UISetting : IMarkableForDelete
    {
        public UISetting()
        {
            ID = Guid.NewGuid();
        }
        
        public Guid ID { get; init; }
        public string Name { get; set; }
        public string Note { get; set; }
        public byte ProviderType { get; set; }
        public long ObjectType { get; set; }
        public byte LocationMode { get; set; }
        public bool RestoreRemovedUsers { get; set; }
        public byte[] RowVersion { get; set; }
        public Guid? LocationItemID { get; set; }
        public bool UpdateLocation { get; set; }
        public bool UpdateSubdivision { get; set; }
        public byte OrganizationComparison { get; set; }
        public byte SubdivisionComparison { get; set; }
        public byte UserComparison { get; set; }
        public int? SubdivisionDefaultOrganizationItemClassID { get; set; }
        public Guid? SubdivisionDefaultOrganizationItemID { get; set; }
        public Guid? UserDefaultOrganizationItemID { get; set; }
        public bool UserAddedWorkflowEnabled { get; set; }
        public string UserAddedWorkflowSchemeIdentifier { get; set; }
        public bool UserModifiedWorkflowEnabled { get; set; }
        public string UserModifiedWorkflowSchemeIdentifier { get; set; }
        public bool UserRemovedWorkflowEnabled { get; set; }
        public string UserRemovedWorkflowSchemeIdentifier { get; set; }
        public bool DivisionAddedWorkflowEnabled { get; set; }
        public string DivisionAddedWorkflowSchemeIdentifier { get; set; }
        public bool DivisionModifiedWorkflowEnabled { get; set; }
        public string DivisionModifiedWorkflowSchemeIdentifier { get; set; }
        public bool OrganizationAddedWorkflowEnabled { get; set; }
        public string OrganizationAddedWorkflowSchemeIdentifier { get; set; }
        public bool OrganizationModifiedWorkflowEnabled { get; set; }
        public string OrganizationModifiedWorkflowSchemeIdentifier { get; set; }
        public bool InquiryTaskWorkflowEnabled { get; set; }
        public string InquiryTaskWorkflowSchemeIdentifier { get; set; }
        
        public virtual UICSVSetting UICSVSetting { get; set; }
        public virtual UIADSetting UIADSetting { get; set; }
        
        public bool Removed { get; private set; }
        public void MarkForDelete()
        {
            Removed = true;
        }
    }
}
