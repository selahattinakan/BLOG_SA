using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BLOG_SA.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HandleErrorCode(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.Message = "Page Not Found!";
                    break;
                case 500:
                    ViewBag.Message = "Internal Server Error";
                    break;
                default:
                    ViewBag.Message = "Error!";
                    break;
            }
            //log şişmesin - sunucu :(
            //_logger.LogError("Status Code :{statusCode}, Request Path: {path}", statusCode, statusCodeData.OriginalPath);
            return View();
        }
    }
}
