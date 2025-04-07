using System;

namespace InfraManager.UI.Web.Models.Configuration;

public class ServiceSettings
{
    public ServiceSettings(string host)
    {
        var url = new Uri(host);

        HostName = url.Host;
        Port = url.Port;
        IsSecure = url.Scheme == Uri.UriSchemeHttps;
    }
    
    public string HostName { get; set; }
    public int Port { get; set; }
    public bool IsSecure { get; set; }

    public override string ToString()
    {
        var protocol = IsSecure ? "https" : "http";
        return $"{protocol}://{HostName}:{Port}";
    }
}
