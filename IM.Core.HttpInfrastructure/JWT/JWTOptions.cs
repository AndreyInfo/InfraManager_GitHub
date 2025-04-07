namespace IM.Core.HttpInfrastructure.JWT;

public class JWTOptions
{
    public bool RequireHttpsMetadata { get; set; }
    public bool ValidateIssuer { get; set; }
    public string ValidIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public string ValidAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public string IssuerSigningKey { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
}