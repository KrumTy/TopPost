namespace TopPost.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using TopPost.Data;
    using TopPost.Data.Common;
    using TopPost.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return this.View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return this.View();
        }        

        public ActionResult Display()
        {
            return this.View();
        }

        public ActionResult Error()
        {
            return this.View();
        }
    }
}