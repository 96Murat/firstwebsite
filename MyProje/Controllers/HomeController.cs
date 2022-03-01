using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyProje.BusinessLogic;
using MyProje.Models;
using MyProje.Models.User;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserBL user;
        private readonly CityBL city;

        public HomeController(ILogger<HomeController> logger, UserBL _user, CityBL _city)
        {
            _logger = logger;
            user = _user;
            city = _city;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.IsLoggined = HttpContext.Session.GetString("IsLoggined");
            ViewBag.IsAdmin = HttpContext.Session.GetString("IsAdmin");
            ViewBag.UName = HttpContext.Session.GetString("UName");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
