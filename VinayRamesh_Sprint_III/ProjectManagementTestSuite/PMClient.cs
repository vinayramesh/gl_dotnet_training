using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using ProjectManagement;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace ProjectManagementTestSuite
{
    public class PMClient: WebApplicationFactory<Startup>
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
        }
    }
}
