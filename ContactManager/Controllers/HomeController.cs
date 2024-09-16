using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactManager.Controllers
{
    public class HomeController : Controller
    {
        [Route("Home/Error")]
        public IActionResult Error()
        {
            return View();
        }
    }

}
