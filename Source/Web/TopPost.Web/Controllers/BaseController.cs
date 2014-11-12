namespace TopPost.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using TopPost.Data;
    using TopPost.Data.Common;
    using TopPost.Models;

    public class BaseController : Controller
    {
        protected TopPostData db = TopPostData.Create();

        protected ApplicationUser GetUser()
        {
            return this.db.Users.All().FirstOrDefault(x => x.UserName == User.Identity.Name);
        }
    }
}