namespace IM.Core.Import.BLL.Import;

public static class ImportConstants
{
    public static int DefaultUserWorkplaceID => 0;
    public static int DefaultUserRoomID => 0;

    public static Guid? DefaultOrganizationID => new Guid("00000000-0000-0000-0000-000000000001");
    public static Guid? DefaultSubdivisionID => new Guid("00000000-0000-0000-0000-000000000001");

    public static int DefaultJobTitle => 0;
}