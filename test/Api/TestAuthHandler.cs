using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StrawberryTesting.Tests.Api
{
    public static class TestUsers 
    {
        public static UserContext BloggerSimon = new("simon", "Blogger");
        public static UserContext BloggerMicheal = new("micheal", "Blogger");
        public static UserContext ReaderAndi = new ("Andi", "Reader");

        public static List<UserContext> All = new()
        {
            BloggerMicheal,
            BloggerSimon,
            ReaderAndi
        };
    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Context.Request.Headers.TryGetValue("user", out var userIdString);

            var user = TestUsers.All.FirstOrDefault(u => u.UserId == userIdString);
            if (user is null)
            {
                // not authenticated!
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            
            var identity = CreateIdentity(user);
             

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Bearer");
            var result = AuthenticateResult.Success(ticket);
            return Task.FromResult(result);
        }

        private static ClaimsIdentity CreateIdentity(UserContext userContext )
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userContext.UserId),
                new Claim(ClaimTypes.NameIdentifier, userContext.UserId),
                new Claim("Role", userContext.Role)
            };

            return new ClaimsIdentity(claims, "Bearer");
        }
    }
}