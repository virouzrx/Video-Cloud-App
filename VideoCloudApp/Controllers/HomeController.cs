using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VideoCloudApp.Areas.Identity.Data;
using VideoCloudApp.Models;

namespace VideoCloudApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly VideoCloudAppDatabaseContext _context;
        private readonly UserManager<VideoCloudAppUser> _currentUser;

        public HomeController(VideoCloudAppDatabaseContext Context, UserManager<VideoCloudAppUser> CurrentUser)
        {
            _context = Context;
            _currentUser = CurrentUser;
        }

        public async Task<ActionResult> Index()
        {
            var Videos = await _context.Videos
                .Where(v => v.AppUserId == _currentUser.GetUserId(User))
                .ToListAsync();

            return View(Videos);
        }

        public IActionResult Upload()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
