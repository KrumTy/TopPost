using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopPost.Data;
using TopPost.Models;

namespace TopPost.MVC.Controllers
{
    public class LikesController : BaseController
    {
        // GET: Likes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Vote(Like vote)
        {
            vote.UserId = this.GetUser().Id;
            var post = this.db.Posts.Find(vote.PostId);

            UnvoteIfPosible((int)vote.PostId, post);

            this.db.Likes.Add(vote);
            db.SaveChanges();

            return RedirectToAction("Show", "Posts", new { id = vote.PostId }); // TODO: Fix redirection
        }

        public ActionResult Unvote(int postId)
        {
            var post = this.db.Posts.Find(postId);

            UnvoteIfPosible(postId, post);

            return RedirectToAction("Show", "Posts", new { id = post.Id });
        }

        private void UnvoteIfPosible(int postId, Post post)
        {
            var user = this.GetUser();

            foreach (var vote in post.Likes)
            {
                if (vote.UserId == user.Id)
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

            foreach (var like in post.Likes)
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

        public string GetCurrentUserValue(int postId)
        {
            var user = this.GetUser();
            var post = this.db.Posts.Find(postId);

            foreach (var vote in post.Likes)
            {
                if (vote.UserId == user.Id)
                {
                    return vote.Value == true ? "Up" : "Down";
                }
            }

            return "";
        }
    }
}