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
        Result result = new Result();

        public ViewResult Index()
        {
            IEnumerable<Item> items = result.ResultList;
            ViewBag.Items = items;
            return View();
        }
    }
}