using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoCloudApp.Services
{
    public class VideoFilePaths
    {
        public string OriginalFilePath { get; set; }
        public string FileDirectory { get; set; }
        public string FileGuid { get; set; }

        public VideoFilePaths(string OriginalFilePath, string FileDirectory, string FileGuid)
        {
            this.OriginalFilePath = OriginalFilePath;
            this.FileDirectory = FileDirectory;
            this.FileGuid = FileGuid;
        }
    }
}
