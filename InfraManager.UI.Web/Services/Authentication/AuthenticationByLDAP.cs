using InfraManager.BLL.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System;
using System.Linq;

namespace InfraManager.UI.Web.Services.Authentication
{
    public class AuthenticationByLDAP : IAuthenticationService
    {
        private const string LDAP_Attribute_RootDomainName = "rootDomainNamingContext";
        private const string LDAP_Filter_SearchUserByLogin = "(&(objectClass=user)(objectClass=person)(sAMAccountName={0}))";
        private const string LDAP_IsDisabled = "514";
        private const string LDAP_Attribute_IsUserDisabled = "userAccountControl";

        private readonly ILogger<AuthenticationByLDAP> _logger;
        private readonly IConfiguration _configuration;

        public AuthenticationByLDAP(ILogger<AuthenticationByLDAP> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public bool IsValid(string login, string password)
        {
            if(String.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if(String.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var userDomain = GetDomain(login);
            var ldapInformation = _configuration.GetSection("LDAP").Get<LDAPConfiguration>();
            
            var ldapCredentials = ldapInformation.LDAPCredentials.FirstOrDefault(x =>
                string.Equals(x.NetBiosName, userDomain, StringComparison.CurrentCultureIgnoreCase));

            if (ldapCredentials == null)
            {
                _logger.LogError("LDAP Credentials was not found");
                throw new NotSupportedException("LDAP Credentials was not found");
            }
            
            try
            {
                using var ldapConn = new LdapConnection
                    { SecureSocketLayer = ldapCredentials.LDAPSecureSocketLayer };

                ldapConn.Connect(ldapCredentials.LDAPIp, ldapCredentials.LDAPPort);
                ldapConn.Bind(login, password);

                if (!ldapConn.Bound)
                {
                    _logger.LogError("No connection to LDAP service");

                    return false;
                }

                var searchBase = GetDomainName(ldapConn);

                if (string.IsNullOrWhiteSpace(searchBase))
                {
                    _logger.LogError("A domain name not found in LDAP service.");

                    return false;
                }

                var loginWithoutDomain = GetLoginWithoutDomain(login);
                var searchScope = LdapConnection.ScopeSub;
                var searchFilter = string.Format(LDAP_Filter_SearchUserByLogin, loginWithoutDomain);
                var searchResults = ldapConn.Search(searchBase, searchScope, searchFilter, null, false);


                var nextEntry = searchResults.Next();

                nextEntry.GetAttributeSet();
                var attr = nextEntry.GetAttribute(LDAP_Attribute_IsUserDisabled);

                if (attr != null && attr.StringValue == LDAP_IsDisabled)
                {
                    _logger.LogInformation($"The user account: {login} is disabled.");

                    return false;
                }

                _logger.LogInformation($"The user account: {login} is enabled.");

                return true;
            }
            catch (LdapReferralException)
            {
                _logger.LogInformation($"The user: {login} not found.");

                return false;
            }
            catch (LdapException ex)
            {
                _logger.LogError(ex, $"{ex.Message}, LOGIN: {login}");

                return false;
            }
        }

        public string GetLoginWithoutDomain(string login)
        {
            if(String.IsNullOrWhiteSpace(login))
            {
                return null;
            }

            int pos = login.IndexOf('\\');

            return pos != -1 ? login.Substring(pos + 1) : login;
        }
        
        public string GetDomain(string login)
        {
            if(String.IsNullOrWhiteSpace(login))
            {
                return null;
            }

            var pos = login.IndexOf('\\');

            return pos != -1 ? login.Substring(0, pos) : login;
        }

        private string GetDomainName(ILdapConnection ldapConnection)
        {
            var domainSearchResults = ldapConnection.Search(String.Empty, LdapConnection.ScopeBase, String.Empty, null, false);
            
            try
            {
                var nextEntry = domainSearchResults.Next();

                nextEntry.GetAttributeSet();
                var attr = nextEntry.GetAttribute(LDAP_Attribute_RootDomainName);

                if (attr != null)
                {
                    return attr.StringValue;
                }

                return null;
            }
            catch (LdapException ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }
        }
    }


    public class LDAPConfiguration
    {
        public bool LDAP_Authentication { get; set; }
        public LDAPCredentials[] LDAPCredentials { get; set; }
    }

    public class LDAPCredentials
    {
        public string LDAPIp { get; set; }
        public int LDAPPort { get; set; } 
        public bool LDAPSecureSocketLayer { get; set; }
        public string NetBiosName { get; set; }
    }
}
