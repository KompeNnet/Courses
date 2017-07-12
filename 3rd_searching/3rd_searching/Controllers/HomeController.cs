using _3rd_searching.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace _3rd_searching.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string arg)
        {
            List<Item> result = new BelChip().Parse(arg);
            result.AddRange(new ChipDip().Parse(arg));
            ViewBag.items = result;
            return View();
        }
    }
}