using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View("dupa");
        }

        // 
        // GET: /HelloWorld/Welcome/ 

        public ActionResult Welcome(string name = null, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;

            return View();
        }
    }
}