using Novell.Directory.Ldap;

namespace IM.Core.Import.BLL.Ldap;

public enum LdapScopeEnum:int
{
    Base = LdapConnection.ScopeBase,
    One = LdapConnection.ScopeOne,
}