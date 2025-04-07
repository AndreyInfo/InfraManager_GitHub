using IM.Core.Import.BLL.Interface.Import;

namespace IM.Core.Import.BLL.OrganizationStructure;

public class ErrorStatistics<TDetails>
{
    public int CreateCount { set; get; } = 0;

    public int UpdateCount { set; get; } = 0;

    public int UpdatedErrors { set; get; } = 0;

    public int CreatedErrors { set; get; } = 0;
    
    public ErrorDetails<TDetails> ErrorDetails { set; get; } = new();
}