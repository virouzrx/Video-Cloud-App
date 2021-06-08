﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoCloudApp.Models
{
    public class AppUser : IdentityUser
    {
        public virtual ICollection<VideoModel> Videos { get; set; }
    }
}
