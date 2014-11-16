    namespace TopPost.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using AutoMapper.QueryableExtensions;

    using TopPost.Data;
    using TopPost.Data.Common;
    using TopPost.Data.Common.Repositories;
    using TopPost.Models;
    using TopPost.Web.ViewModels.Comments;
    using TopPost.Web.ViewModels.Posts;

    public class CommentsController : BaseController
    {
        private readonly IDeletableEntityRepository<Comment> comments;

        public CommentsController(IDeletableEntityRepository<Comment> comments, ITopPostData data)
            : base(data)
        {
            this.comments = comments;
        }

        // GET: Comments
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Post(CommentInputModel comment)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var userId = this.GetUser().Id;

                    var newComment = new Comment
                    {
                        PostId = comment.PostId,
                        Text = comment.Text, // sanitizer.Sanitize(input.Content),
                        AuthorId = userId,
                        ParentCommentId = comment.ParentCommentId
                    };

                    this.comments.Add(newComment);
                    this.comments.SaveChanges();

                    return this.GetPostComments(newComment.PostId);
                }

                return this.PartialView("_SubmitCommentPartial", comment);
            }

            else return RedirectToAction("Login", "Account");
        }

        public ActionResult GetComments(int commentId)
        {
            var comments = this.comments
                .Find(commentId)
                .Comments
                .AsQueryable()
                .Project()
                .To<CommentDetailViewModel>()
                .ToList();

            return this.PartialView("_ViewRepliesPartial", comments);
        }

        public ActionResult GetReplyPartial(int commentId)
        {
            var comment = this.comments.Find(commentId);

            CommentInputModel reply = new CommentInputModel()
            {
                ParentCommentId = comment.Id,
                PostId = comment.PostId
            };

            return this.PartialView("_SubmitCommentPartial", reply);
        }

        public ActionResult GetCommentPartial(int postId)
        {
            CommentInputModel reply = new CommentInputModel()
            {
                PostId = postId
            };

            return this.PartialView("_SubmitCommentPartial", reply);
        }

        public ActionResult GetPostComments(int postId, int take = 30)
        {
            var comments = this.comments
                .All()
                .AsQueryable()
                .Where(x => x.PostId == postId && x.ParentCommentId == null)
                .OrderByDescending(p => p.Likes.Where(l => l.Value == true && !l.IsDeleted).Count() - p.Likes.Where(l => l.Value == false && !l.IsDeleted).Count())
                .Take(take)
                .Project()
                .To<CommentDetailViewModel>()
                .ToList();

            return this.PartialView("_ViewRepliesPartial", comments);
        }
    }
}