namespace IM.Core.Import.BLL.Interface.Import;

public record SubdivisionNameKey
{
    public string Name { get; set; }
        
    public Guid? OrganizationID { get; set; }
}