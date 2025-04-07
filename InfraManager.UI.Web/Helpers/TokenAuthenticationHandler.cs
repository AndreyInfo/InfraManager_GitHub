using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Security.Claims;
using System.Net;
using Newtonsoft.Json;
using InfraManager.WebAPIClient;

namespace InfraManager.UI.Web.Helpers
{
    public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TokenAuthenticationOptions _authOptions;
        private readonly ILogger<TokenAuthenticationHandler> _logger;

        public TokenAuthenticationHandler(
                    IOptionsMonitor<AuthenticationSchemeOptions> optionMonitor,
                    ILoggerFactory logger,
                    UrlEncoder encoder,
                    ISystemClock clock,
                    IOptions<TokenAuthenticationOptions> confAuthOptions)
            : base(optionMonitor, logger, encoder, clock)
        {
            this._logger = logger.CreateLogger<TokenAuthenticationHandler>();
            this._authOptions = confAuthOptions.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult authResult = null;

            // skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() == null)
            {
                if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
                    authResult = Fail("Missing Authorization Header");
                else
                {
                    JwtSecurityToken jwtToken = null;
                    try
                    {
                        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers[HeaderNames.Authorization]);
                        var decodeState = TokenUtility.TryDecodeToken(authHeader.Parameter, _authOptions.TokenPassword, out jwtToken);
                        if (decodeState == TokenUtility.TokenState.Valid)
                        {
                            if (jwtToken.Payload == null ||
                                !jwtToken.Payload.ContainsKey("id"))
                                throw new Exception("Wrong payload");
                        }
                        else if (decodeState == TokenUtility.TokenState.Expired)
                            authResult = Fail("JWT token expired");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Invalid Authorization Header");
                        authResult = Fail("Invalid Authorization Header");
                    }
                    if (authResult == null)
                    {
                        var claims = new[] { jwtToken.Claims.First(x => x.Type == "id") };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        authResult = AuthenticateResult.Success(ticket);
                    }
                }
            }
            else
            {
                authResult = AuthenticateResult.NoResult();
            }
            return Task.FromResult(authResult);
        }

        private AuthenticateResult Fail(string errorReason)
        {
            var errorFeature = new TokenAuthenticationErrorFeature(errorReason);
            Context.Features.Set<ITokenAuthenticationErrorFeature>(errorFeature);
            return AuthenticateResult.Fail(errorFeature.ErrorReason);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers[HeaderNames.WWWAuthenticate] = $"{Scheme.Name} token";

            var errorFeature = Context.Features.Get<ITokenAuthenticationErrorFeature>();
            var apiError = new ApiError(HttpStatusCode.Unauthorized)
            {
                Path = Request.Path,
                Message = errorFeature?.ErrorReason
            };

            Response.ContentType = "application/json";
            Response.StatusCode = apiError.Status;
            await Response.WriteAsync(JsonConvert.SerializeObject(apiError));
        }
    }
}
