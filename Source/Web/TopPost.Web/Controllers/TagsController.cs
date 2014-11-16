namespace TopPost.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using TopPost.Data;

    public class TagsController : BaseController
    {
        public TagsController(ITopPostData data)
            :base(data)
        {

        }

        // GET: Tags
        public ActionResult Index(string tagName)
        {
            var tag = this.db.Tags.All().FirstOrDefault(t => t.Name == tagName);

            return this.View(tag);
        }
    }
}