using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;
using System.Diagnostics;

namespace MyShop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext database ,ILogger<HomeController> logger)
        {
            _db = database;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var user  = _db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();

            return View(user);
        }

        [AllowAnonymous]
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