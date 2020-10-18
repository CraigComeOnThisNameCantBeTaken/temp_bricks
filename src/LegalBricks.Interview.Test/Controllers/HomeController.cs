using Microsoft.AspNetCore.Mvc;
using NHibernate;

namespace LegalBricks.Interview.Test.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
