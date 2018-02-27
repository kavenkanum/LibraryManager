using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMVC.Controllers
{
    public class HelloWorldController : Controller
    {
        public string Index()
        {
            return "This is my <b>default</b> action...";
        }
        
        // 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome(string name = null, int id = 1)
        {
            return HttpUtility.HtmlEncode("Hello " + name + ", ID is: " + id);
        }
    }
}