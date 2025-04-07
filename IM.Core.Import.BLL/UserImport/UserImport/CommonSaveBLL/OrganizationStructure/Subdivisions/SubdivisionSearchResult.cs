namespace IM.Core.Import.BLL.Import;

public record SubdivisionSearchResult(Guid? LastFoundID, IEnumerable<string> GetNotFoundIDs);
