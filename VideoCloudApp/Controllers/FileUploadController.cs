using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoCloudApp.Services;
using VideoCloudApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using VideoCloudApp.Models;

namespace VideoCloudApp.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly VideoCloudAppDatabaseContext _context;
        private readonly UserManager<VideoCloudAppUser> _currentUser;

        public FileUploadController(IWebHostEnvironment Environment, VideoCloudAppDatabaseContext Context, UserManager<VideoCloudAppUser> CurrentUser)
        {
            _environment = Environment;
            _context = Context;
            _currentUser = CurrentUser;
        }

        public IActionResult Index()
        {
            return View(new VideoModel());
        }
        [HttpGet("FileUpload")]
        public IActionResult FileUpload()
        {
            return View(new VideoModel());
        }

        [HttpPost("FileUpload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> FileUpload(List<IFormFile> files, string title, string description)
        {

            VideoConversion vc = new();
            ServerOperations so = new();
            string path = Path.Combine(_environment.WebRootPath, "uploads"); //c/nanana/projekt/nanana/uploads

            foreach (var file in files)
            {
                var VideoFilePaths = await so.SaveFileOnServer(file, path); //c/nanana/projekt/nanana/uploads/guid/file.mp4
                var paths = await vc.DoAllConversions(VideoFilePaths.FileDirectory, VideoFilePaths.OriginalFilePath, VideoFilePaths.FileGuid);

                var currentuserid = _currentUser.GetUserId(User);
                VideoModel vm = new()
                {
                    AppUserId = currentuserid,
                    Title = title,
                    Description = description,
                    UploadDate = DateTime.Now,
                    VideoOriginalFilePath = VideoFilePaths.OriginalFilePath,
                    VideoBasicFilePath = paths[0],
                    Video480Path = paths[480],
                    Video720Path = paths[720],
                    Video1080Path = paths[1080]
                };
                await _context.AddAsync(vm);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}



