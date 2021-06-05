using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoCloudApp.Areas.Identity.Data;

[assembly: HostingStartup(typeof(VideoCloudApp.Areas.Identity.IdentityHostingStartup))]
namespace VideoCloudApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            { 
                services.AddDefaultIdentity<VideoCloudAppUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<VideoCloudAppDatabaseContext>();
            });
        }
    }
}