namespace IM.Core.Import.BLL.Interface.Import;

public interface ISubdivisionHierarchyDetails
{
    ISubdivisionDetails? ParentSubdivision { get; set; }
}