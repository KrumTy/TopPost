using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TopPost.MVC.Controllers
{
    public class TagsController : BaseController
    {
        // GET: Tags
        public ActionResult Index(string tagName)
        {
            var tag = this.db.Tags.All().FirstOrDefault(t => t.Name == tagName);

            return View(tag);
        }
    }
}