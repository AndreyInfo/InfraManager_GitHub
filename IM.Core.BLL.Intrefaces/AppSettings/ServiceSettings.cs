using System;

namespace InfraManager.BLL.AppSettings;

public class ServiceSettings
{
    public ServiceSettings()
    {
        
    }
    
    public ServiceSettings(string host)
    {
        var url = new Uri(host);

        HostName = url.Host;
        Port = url.Port;
        IsSecure = url.Scheme == Uri.UriSchemeHttps;
    }
    
    public string HostName { get; init; }
    public int Port { get; init; }
    public bool IsSecure { get; init; }

    public override string ToString()
    {
        var protocol = IsSecure ? "https" : "http";
        return $"{protocol}://{HostName}:{Port}";
    }
}
