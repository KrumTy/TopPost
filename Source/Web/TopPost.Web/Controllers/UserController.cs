namespace TopPost.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using TopPost.Data;

    public class UserController : BaseController
    {
        public UserController(ITopPostData data)
            :base(data)
        {

        }

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

            return this.View();
        }

        public ActionResult Profiles(string username)
        {
            // var username = this.db.Users.All().FirstOrDefault(x => x.UserName == username);
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

            return this.View(); // TODO: Models
        }
    }
}