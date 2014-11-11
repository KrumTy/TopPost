using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopPost.Models;

namespace TopPost.MVC.Controllers
{
    public class FavoritesController : BaseController
    {
        // GET: Favorites
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetFavoriteButton(int postId)
        {
            var userId = this.GetUser().Id;
            var fav = this.db.Posts.Find(postId).Favorites.FirstOrDefault(x => x.UserId == userId);

            if (fav == null)
            {
                fav = new Favorite() { PostId = postId };
            }

            return PartialView("_FavoritePartial", fav);
        }

        public ActionResult Add(int postId)
        {
            var userId = this.GetUser().Id;
            var fav = new Favorite()
            {
                PostId = postId,
                UserId = userId
            };

            this.db.Favorites.Add(fav);
            this.db.SaveChanges();

            return PartialView("_FavoritePartial", fav);
        }

        public ActionResult Remove(int favId)
        {
            var postId = this.db.Favorites.Find(favId).PostId;
            this.db.Favorites.Delete(favId);
            this.db.SaveChanges();

            var fav = new Favorite() { PostId = postId };

            return PartialView("_FavoritePartial", fav);
        }

        public ActionResult GetUserFavorites(string username)
        {
            var user = this.db.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                user = this.GetUser();
            }

            var posts = user.Favorites.Select(x=>x.Post).ToList();

            return PartialView("_ViewPostsPartial", posts);
        }
    }
}