using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopPost.Data;
using TopPost.Models;
using System.Web.Security;
namespace TopPost.MVC.Controllers
{
    public class CommentsController : BaseController
    {
        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post(Comment comment)
        {
            comment.AuthorId = this.GetUser().Id;
            this.db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Show", "Posts", new { id = comment.PostId });
        }
    }
}