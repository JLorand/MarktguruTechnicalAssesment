using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Marktguru.API;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private const string HardcodedUsername = "username";
    private const string HardcodedPassword = "password";
    
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return await Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        }

        try
        {
            var authHeader = authorizationHeader.ToString();
            var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);

            if (!authHeaderValue.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) ||
                authHeaderValue.Parameter == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
            }
            
            var credentials = DecodeCredentials(authHeaderValue.Parameter);
            var username = credentials[0];
            var password = credentials[1];

            if (!ValidCredentials(username, password))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
            }
            
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));

        }
        catch
        {
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
    
    private static string[] DecodeCredentials(string encodedCredentials)
    {
        var decodedBytes = Convert.FromBase64String(encodedCredentials);
        var decodedString = Encoding.UTF8.GetString(decodedBytes);
        return decodedString.Split(':', 2);
    }

    private static bool ValidCredentials(string username, string password)
    {
        return username == HardcodedUsername && password == HardcodedPassword;
    }
}