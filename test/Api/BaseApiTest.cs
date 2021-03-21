using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StrawberryTesting.Tests.Api
{
    public abstract class BaseApiTest
    {
        protected static IWebHost GivenTestHost(Action<IServiceCollection> configureTestServices = null)
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Test")
                .UseTestServer()
                .ConfigureTestServices(services =>
                {
                    services.AddFakeAuthenticationHandler();
                    configureTestServices?.Invoke(services);
                })
                .ConfigureLogging(x =>
                {
                    x.SetMinimumLevel(LogLevel.Trace);
                    x.AddXUnit();
                })
                .Start();
        }

    }
}