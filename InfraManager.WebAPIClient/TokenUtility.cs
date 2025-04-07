using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace InfraManager.WebAPIClient
{
    public static class TokenUtility
    {
        public enum TokenState
        {
            Valid,
            Expired,
            Invalid
        }

        public static bool IsJWTToken(string token)
        {
            return new Regex(JwtConstants.JsonCompactSerializationRegex).IsMatch(token);
        }

        public static string CreateToken(string secret, Guid userId)
        {
            var secureKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(secureKey, SecurityAlgorithms.HmacSha256Signature);

            var myIssuer = "http://inframanager.ru";
            var myAudience = "http://inframanager.ru";
            var utcNow = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("id", userId.ToString()),
                        }),
                Expires = utcNow.AddMinutes(7),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public static TokenState TryDecodeToken(string token, string secret, out JwtSecurityToken jwtToken)
        {
            jwtToken = null;
            var tokenHandler = new JwtSecurityTokenHandler();

            // CreateToken(secret);
            try
            {
                var secret_key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secret_key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                jwtToken = tokenHandler.ReadJwtToken(token);

                tokenHandler.ValidateToken(token, validations, out var tokenSecure);

                return TokenState.Valid;
            }
            catch (SecurityTokenExpiredException)
            {
                return TokenState.Expired;
            }
            catch (Exception)
            {
                return TokenState.Invalid;
            }
        }
    }
}
