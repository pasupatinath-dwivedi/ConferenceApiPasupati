using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ConferenceApi.Helper
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
                                                ISystemClock clock, IConfiguration configuration)
                                : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return ReturnUnAuthenticated();
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                if (!IsAuthenticated(username, password))
                {
                    // Return unauthorized
                    return ReturnUnAuthenticated();
                };
                var claims = new[] {
                new Claim(ClaimTypes.Name, username)           };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.Run(() => AuthenticateResult.Success(ticket));
            }
            catch
            {
                return ReturnUnAuthenticated();
            }
        }

        private Task<AuthenticateResult> ReturnUnAuthenticated()
        {
            Response.Headers["WWW-Authenticate"] = "Basic";
            Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            return Task.Run(() => AuthenticateResult.Fail("error"));
        }

        /// <summary>
        /// Authenticate if user name and password is correct
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool IsAuthenticated(string username, string password)
        {
            var basicAuthUserName =  _configuration.GetValue<string>("BasicAuthentication:UserName");
            var basicAuthPassword = _configuration.GetValue<string>("BasicAuthentication:Password");
            // Check that username and password are correct
            return username.Equals(basicAuthUserName, StringComparison.InvariantCultureIgnoreCase)
                   && password.Equals(basicAuthPassword);
        }
    }
}
