using System;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using StrawberryShakeTesting.Tests;

namespace StrawberryTesting.Tests.Api
{
    public static class TestingExtensions
    {
        public static BlogClient GetBlogClient(this IWebHost host, UserContext user = null)
        {
            var client = host.GetTestClient();
            client.BaseAddress = new Uri("http://localhost/graphql/"); // proper base address needs to be set
            if(user is {})
                client.DefaultRequestHeaders.Add("user", user.UserId);
        
            var services = new ServiceCollection();
            services.AddScoped(sp =>
            {
                var httpClientFactory = Substitute.For<IHttpClientFactory>();
                httpClientFactory.CreateClient().Returns(client);
                httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client); // important otherwise HttpConnection can't resolve
                return httpClientFactory;
            });
            services.AddBlogClient();
            
            return services.BuildServiceProvider()
                .GetRequiredService<BlogClient>();
        }

        public static AuthenticationBuilder AddFakeAuthenticationHandler(this IServiceCollection services)
        {
            return services.AddAuthentication("Bearer")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Bearer", options => { });
        }
    }
}