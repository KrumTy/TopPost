using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TopPost.MVC.Controllers
{
    public class UserController : BaseController
    {
        // GET: Profile
        public ActionResult Index(string username)
        {
            if (username == null)
            {
                ViewBag.username = this.User.Identity.Name;
            }
            else
            {
                ViewBag.username = username;
            }

            return View();
        }

        public ActionResult Profiles(string username)
        {
            //var username = this.db.Users.All().FirstOrDefault(x => x.UserName == username);
            if (username == null)
            {
                ViewBag.username = this.User.Identity.Name;
            }
            else
            {
                var user = this.db.Users.All().FirstOrDefault(u => u.UserName == username);
                ViewBag.username = username;

                if (user == null)
                {
                    // redirect
                    ViewBag.username = this.User.Identity.Name;
                }
            }

            return View(); // TODO: Models
        }
    }
}