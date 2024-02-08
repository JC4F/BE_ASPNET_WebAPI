using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("/blog/{id}")]
        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Manage()
        {
            return View();
        }
    }
}
