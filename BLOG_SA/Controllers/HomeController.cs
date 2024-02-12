using BLOG_SA.Models;
using DB_EFCore.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BLOG_SA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Views
        public IActionResult Index()
        {
            return View();
        }

		public IActionResult Article()
		{
			return View();
		}

		public IActionResult Contact()
        {
            return View();
        }

        public IActionResult RSS()
        {
            return View();
        }

        public IActionResult Subscribe()
        {
            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
