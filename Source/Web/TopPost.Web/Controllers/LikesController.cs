namespace TopPost.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using TopPost.Data;
    using TopPost.Data.Common;
    using TopPost.Models;
    using TopPost.Web.ViewModels.Vote;
    using TopPost.Web.ViewModels.Posts;

    public class LikesController : BaseController
    {
        public LikesController(ITopPostData data)
            :base(data)
        {

        }

        // GET: Likes
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult GetCommentLikes(int commentId)
        {
            var userId = this.GetUser().Id;
            var like = this.db.Comments
                .All()
                .FirstOrDefault(c => c.Id == commentId)
                .Likes
                .FirstOrDefault(x => x.UserId == userId && !x.IsDeleted);

            if (like == null)
            {
                like = new Like() { CommentId = commentId };
            }

            var vote = Mapper.Map<VoteViewModel>(like);

            return this.PartialView("CommentLikesPartial", vote);
        }

        public ActionResult VoteComment(Like like)
        {
            like.UserId = this.GetUser().Id;
            var comment = this.db.Comments.Find(like.CommentId);

            var currentVote = comment.Likes.FirstOrDefault(x => !x.IsDeleted && x.UserId == like.UserId);

            if (currentVote == null)
            {
                currentVote = like;
                this.db.Likes.Add(currentVote);
            }
            else
            {
                currentVote.Value = like.Value;
                this.db.Likes.Update(currentVote);
            }
            
            db.SaveChanges();

            var vote = Mapper.Map<VoteViewModel>(currentVote);

            return this.PartialView("CommentLikesPartial", vote);
        }

        public ActionResult UnvoteComment(int commentId)
        {
            var comment = this.db.Comments.Find(commentId);
            var user = this.GetUser();
            var currentVote = comment.Likes.FirstOrDefault(x => !x.IsDeleted && x.UserId == user.Id);
            db.Likes.Delete(currentVote.Id);
            db.SaveChanges();            

            var like = new Like() { CommentId = comment.Id };

            var vote = Mapper.Map<VoteViewModel>(like);

            return this.PartialView("CommentLikesPartial", vote);
        }

        public int GetCommentScore(int commentId)
        {
            var comment = this.db.Comments.Find(commentId);
            int score = 0;

            foreach (var like in comment.Likes.Where(l => !l.IsDeleted))
            {
                if (like.Value == true)
                {
                    score++;
                }
                else
                {
                    score--;
                }
            }

            return score;
        }

        public ActionResult GetLikes(int postId)
        {
            var userId = this.GetUser().Id;
            var like = this.db.Posts.Find(postId).Likes.FirstOrDefault(x => x.UserId == userId && !x.IsDeleted);

            if (like == null)
            {
                like = new Like() { PostId = postId };
            }

            var vote = Mapper.Map<VoteViewModel>(like);

            return this.PartialView("_LikePartial", vote);
        }

        public ActionResult Vote(Like like)
        {
            like.UserId = this.GetUser().Id;
            var post = this.db.Posts.Find(like.PostId);

            this.UnvoteIfPosible(post);

            this.db.Likes.Add(like);
            db.SaveChanges();

            var vote = Mapper.Map<VoteViewModel>(like);

            return this.PartialView("_LikePartial", vote);
        }

        public ActionResult Unvote(int postId)
        {
            var post = this.db.Posts.Find(postId);

            this.UnvoteIfPosible(post);

            var like = new Like() { PostId = post.Id };

            var vote = Mapper.Map<VoteViewModel>(like);

            return this.PartialView("_LikePartial", vote);
        }

        public int GetScore(int postId)
        {
            var post = this.db.Posts.Find(postId);
            int score = 0;

            foreach (var like in post.Likes.Where(l => !l.IsDeleted))
            {
                if (like.Value == true)
                {
                    score++;
                }
                else
                {
                    score--;
                }
            }

            return score;
        }

        public ActionResult GetUserLikes(string username)
        {
            var user = this.db.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                user = this.GetUser();
            }

            var posts = user.Likes.AsQueryable().Where(x => !x.IsDeleted && x.Value == true).Select(x => x.Post)
                .Project().To<PostThumbViewModel>().ToList();

            return this.PartialView("_ViewPostsPartial", posts);
        }

        private void UnvoteIfPosible(Post post)
        {
            var user = this.GetUser();

            foreach (var vote in post.Likes)
            {
                if (vote.UserId == user.Id && !vote.IsDeleted)
                {
                    this.db.Likes.Delete(vote.Id);
                    this.db.SaveChanges();
                    break;
                }
            }
        }
    }
}