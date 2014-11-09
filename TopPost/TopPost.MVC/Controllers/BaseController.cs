namespace TopPost.MVC.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using TopPost.Data;
    using TopPost.Models;

    public class BaseController : Controller
    {
        protected ApplicationSystemData db = ApplicationSystemData.Create();

        protected ApplicationUser GetUser()
        {
            return this.db.Users.All().FirstOrDefault(x => x.UserName == User.Identity.Name);
        }
    }
}