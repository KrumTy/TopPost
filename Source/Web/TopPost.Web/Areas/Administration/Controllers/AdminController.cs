namespace TopPost.Web.Areas.Controllers
{
    using System.Web.Mvc;
    using TopPost.Common;
    using TopPost.Data;
    using TopPost.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdminRole)]
    public abstract class AdminController : BaseController
    {
        public AdminController(ITopPostData data)
            : base(data)
        {

        }
    }
}