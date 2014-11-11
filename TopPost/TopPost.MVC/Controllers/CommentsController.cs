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

        public ActionResult GetComments(int commentId)
        {
            var comments = this.db.Comments.Find(commentId).Comments.ToList();

            return PartialView("_ViewRepliesPartial", comments);
        }

        public ActionResult GetReplyPartial(int commentId)
        {
            var comment = this.db.Comments.Find(commentId);

            return PartialView("_SubmitCommentPartial", comment);
        }

        public ActionResult GetCommentPartial(int postId)
        {
            var comment = new Comment() { PostId = postId };

            return PartialView("_SubmitCommentPartial", comment);
        }

        public ActionResult GetCommentWindow(int postId)
        {
            var comment = new Comment() { PostId = postId };

            return PartialView("_SubmitCommentPartial", comment);
        }

        public ActionResult GetPostComments(int postId)
        {
            var comments = this.db.Comments
                .All()
                .AsQueryable()
                .Where(x => x.PostId == postId && x.ParentCommentId == null)
                .ToList();

            return PartialView("_ViewRepliesPartial", comments);
        }
    }
}