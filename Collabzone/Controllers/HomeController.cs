using System.Diagnostics;
using Collabzone.Models;
using Microsoft.AspNetCore.Mvc;

namespace Collabzone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Displays the main homepage view
        public IActionResult Index()
        {
            return View();
        }

        // Displays the privacy policy view
        public IActionResult Privacy()
        {
            return View();
        }

        // Error page handler
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
