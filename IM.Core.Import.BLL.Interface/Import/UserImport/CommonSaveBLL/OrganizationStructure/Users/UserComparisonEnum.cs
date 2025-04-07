namespace IM.Core.Import.BLL.Import;

public enum UserComparisonEnum:byte
{
    ByFullName = 0,
    ByFirstNameLastName = 1,
    ByNumber = 2,
    ByLogin = 3,
    BySID = 4,
    ByExternalID = 5
}