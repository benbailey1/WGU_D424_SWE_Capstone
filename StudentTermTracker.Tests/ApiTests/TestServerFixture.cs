using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace StudentTermTracker.Tests.ApiTests
{
    public class TestServerFixture : IDisposable
    {
        public TestServer Server { get; }
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            // Replace YourApiProject.Startup with your actual API startup class
            var builder = new WebHostBuilder()
                .UseStartup<StudentTermTrackerAPI.Auth.Controllers.UserAccountController>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
    // public class TestServerFixture : IDisposable
    // {
    //     private readonly WebApplicationFactory<StudentTermTrackerAPI.Auth.Controllers.UserAccountController> _factory;
    //     public HttpClient Client { get; }

    //     public TestServerFixture()
    //     {
    //         _factory = new WebApplicationFactory<StudentTermTrackerAPI.Auth.Controllers.UserAccountController>()
    //             .WithWebHostBuilder(builder =>
    //             {
    //                 builder.ConfigureServices(services =>
    //                 {
    //                     // TODO: Create mock services or configure test services here
    //                 });
    //             });
    //     }

    //     public void Dispose()
    //     {
    //         Client.Dispose();
    //         _factory.Dispose();
    //     }
    // }
}
