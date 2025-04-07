namespace InfraManager.BLL.AccessManagement.AccessPermissions
{
    public class AccessPermissionRightsDetails
    {
        public AccessPermissionRightsDetails(bool? properties, bool? add, bool? delete, bool? update, bool? accessManage)
        {
            HasPropertiesPermissions = properties.GetValueOrDefault();
            HasAddPermissions = add.GetValueOrDefault();
            HasDeletePermissions = delete.GetValueOrDefault();
            HasUpdatePermissions = update.GetValueOrDefault();
            HasAccessManagePermissions = accessManage.GetValueOrDefault();
        }

        public AccessPermissionRightsDetails()
        { }

        public bool HasPropertiesPermissions { get; set; }

        public bool HasAddPermissions { get; set; }

        public bool HasDeletePermissions { get; set; }

        public bool HasUpdatePermissions { get; set; }

        public bool HasAccessManagePermissions { get; set; }
    }
}
