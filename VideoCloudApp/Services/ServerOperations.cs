using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VideoCloudApp.Services
{
    public class ServerOperations
    {
        public async Task<VideoFilePaths> SaveFileOnServer(IFormFile file, string path)
        {
            string fileName = Path.GetFileName(file.FileName);
            Guid guid = Guid.NewGuid();
            string FileDirectory = Path.Combine(path, guid.ToString()); //c/nanana/projekt/nanana/uploads/guid
            string OriginalFilePath = Path.Combine(path, guid.ToString(), fileName); //c/nanana/projekt/nanana/uploads/guid/file.mp4
            Directory.CreateDirectory(FileDirectory);
            using (FileStream stream = new(OriginalFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            VideoFilePaths videoFilePaths = new(OriginalFilePath, FileDirectory, guid.ToString());
            return videoFilePaths;
        }
    }
}
