using System.Text;
using IM.Core.HttpInfrastructure.JWT;
using InfraManager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IM.Core.HttpInfrastructure;

public static class InfrastructureExtensions
{
    public static void AddJWT(this IServiceCollection services, JWTOptions jwtOptions)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = jwtOptions.RequireHttpsMetadata;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtOptions.ValidateIssuer,
                    ValidIssuer = jwtOptions.ValidIssuer,
 
                    ValidateAudience = jwtOptions.ValidateAudience,
                    ValidAudience = jwtOptions.ValidIssuer,
                    ValidateLifetime = jwtOptions.ValidateLifetime,
 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.IssuerSigningKey)),
                    ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                };
            });
    }

    public static void AddCurrentUser(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextProvider, UserContextProvider>();
        services.AddScoped<ICurrentUser, HttpContextCurrentUser>();
    }
}