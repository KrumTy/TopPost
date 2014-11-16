namespace TopPost.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using TopPost.Data;
    using TopPost.Data.Common;
    using TopPost.Models;

    public class BaseController : Controller
    {
        protected ITopPostData db = TopPostData.Create();

        protected ApplicationUser UserProfile { get; private set; }

        public BaseController(ITopPostData data)
        {
            this.db = data;
        }

        protected ApplicationUser GetUser()
        {
            return this.db.Users.All().FirstOrDefault(x => x.UserName == User.Identity.Name);
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.db.Users.All().Where(u => u.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}