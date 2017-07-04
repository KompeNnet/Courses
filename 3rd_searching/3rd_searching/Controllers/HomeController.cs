using _3rd_searching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.WebPages;

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