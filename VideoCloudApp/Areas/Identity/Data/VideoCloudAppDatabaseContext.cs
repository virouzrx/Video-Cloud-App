using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoCloudApp.Areas.Identity.Data;
using VideoCloudApp.Models;

namespace VideoCloudApp.Areas.Identity.Data
{
    public class VideoCloudAppDatabaseContext : IdentityDbContext<VideoCloudAppUser>
    {
        public VideoCloudAppDatabaseContext(DbContextOptions<VideoCloudAppDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<VideoModel> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<AppUser>()
                .HasMany(x => x.Videos)
                .WithOne(x => x.AppUser)
                .HasForeignKey(x => x.AppUserId);

            //builder.Entity<VideoModel>().HasOne(a => a.AppUser).WithMany(v => v.Videos).HasForeignKey(s => s.AppUserId); <--this one is wrong
        }
    }
}
