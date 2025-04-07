using System.Collections.Generic;
using InfraManager.DAL.Sessions;

namespace InfraManager.UI.Web.CookieEvents;

public class LocationProvider
{
    private Dictionary<string, SessionLocationType> _locations = new()
    {
        { "AdminConsole", SessionLocationType.AdminConsole }
    };

    public SessionLocationType GetLocation(string header)
    {
        if (!_locations.ContainsKey(header))
        {
            return SessionLocationType.Web;
        }

        return _locations[header];
    }
}