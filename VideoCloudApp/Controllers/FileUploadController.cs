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
using VideoCloudApp.ViewModels;

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
            return View(new VideoUploadViewModel());
        }

        [HttpPost("FileUpload")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> FileUpload(VideoUploadViewModel videoUploadViewModel)
        {

            VideoConversion vc = new();
            ServerOperations so = new();
            
            string path = Path.Combine(_environment.WebRootPath, "uploads"); //wwwrooot/uploads
            if (ModelState.IsValid)
            {
                foreach (var file in videoUploadViewModel.File)
                {
                    var VideoFilePaths = await so.SaveFileOnServer(file, path);
                    var PosterFilePath = await vc.GetVideoThumbnail(VideoFilePaths.FileDirectory, VideoFilePaths.OriginalFilePath, VideoFilePaths.FileGuid);
                    var paths = await vc.DoAllConversions(VideoFilePaths.FileDirectory, VideoFilePaths.OriginalFilePath, VideoFilePaths.FileGuid);

                    var currentuserid = _currentUser.GetUserId(User);
                    VideoModel vm = new()
                    {
                        AppUserId = currentuserid,
                        Title = videoUploadViewModel.Title,
                        Description = videoUploadViewModel.Description,
                        UploadDate = DateTime.Now,
                        VideoOriginalFilePath = VideoFilePaths.OriginalFilePath,
                        PosterPath = PosterFilePath,
                        VideoBasicFilePath = paths[0],
                        Video480Path = paths[480],
                        Video720Path = paths[720],
                        Video1080Path = paths[1080]
                    };
                    await _context.AddAsync(vm);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(videoUploadViewModel);

        }
    }
}



