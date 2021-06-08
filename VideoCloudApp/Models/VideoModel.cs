using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VideoCloudApp.Models
{
    public class VideoModel
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string PosterPath { get; set; }
        public string VideoOriginalFilePath { get; set; }
        public string VideoBasicFilePath { get; set; }
        public string Video480Path { get; set; }
        public string Video720Path { get; set; }
        public string Video1080Path { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
