using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopPost.Data;
using TopPost.Data.Common;
using TopPost.Models;

namespace TopPost.Web.Controllers
{
    public class LikesController : BaseController
    {
        // GET: Likes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLikes(int postId)
        {
            var userId = this.GetUser().Id;
            var like = this.db.Posts.Find(postId).Likes.FirstOrDefault(x => x.UserId == userId && !x.IsDeleted);

            if (like == null)
            {
                like = new Like() { PostId = postId };
            }

            return PartialView("_LikePartial", like);
        }

        public ActionResult Vote(Like vote)
        {
            vote.UserId = this.GetUser().Id;
            var post = this.db.Posts.Find(vote.PostId);

            UnvoteIfPosible(post);

            this.db.Likes.Add(vote);
            db.SaveChanges();

            return PartialView("_LikePartial", vote);
        }

        public ActionResult Unvote(int postId)
        {
            var post = this.db.Posts.Find(postId);

            UnvoteIfPosible(post);

            var vote = new Like() { PostId = post.Id };

            return PartialView("_LikePartial", vote);
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

        public int GetScore(int postId)
        {
            var post = this.db.Posts.Find(postId);
            int score = 0;

            foreach (var like in post.Likes.Where(l=> !l.IsDeleted))
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

            var posts = user.Likes.Where(l => !l.IsDeleted).Select(x => x.Post).ToList();

            return PartialView("_ViewPostsPartial", posts);
        }
    }
}