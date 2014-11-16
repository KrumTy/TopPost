namespace TopPost.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using TopPost.Models;
    using TopPost.Web.ViewModels.Posts;
    using TopPost.Web.ViewModels.Favorites;
    using TopPost.Data;

    public class FavoritesController : BaseController
    {
        public FavoritesController(ITopPostData data)
            : base(data)
        {

        }

        // GET: Favorites
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult GetFavoriteButton(int postId)
        {
            var userId = this.GetUser().Id;
            var fav = this.db.Posts.Find(postId).Favorites.FirstOrDefault(x => x.UserId == userId && !x.IsDeleted);

            if (fav == null)
            {
                fav = new Favorite() { PostId = postId };
            }

            var favorite = Mapper.Map<FavoriteViewModel>(fav);

            return this.PartialView("_FavoritePartial", favorite);
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

            var favorite = Mapper.Map<FavoriteViewModel>(fav);

            return this.PartialView("_FavoritePartial", favorite);
        }

        public ActionResult Remove(int postId)
        {
            var favorite = this.db.Posts
                .Find(postId)
                .Favorites.FirstOrDefault(x => !x.IsDeleted && x.UserId == this.GetUser().Id);

            db.Favorites.Delete(favorite.Id);
            db.SaveChanges();

            var fav = new FavoriteViewModel() { PostId = postId };

            return this.PartialView("_FavoritePartial", fav);
        }

        public ActionResult GetUserFavorites(string username)
        {
            var user = this.db.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                user = this.GetUser();
            }

            var posts = user.Favorites.AsQueryable().Where(x => !x.IsDeleted).Select(x => x.Post)
                .Project().To<PostThumbViewModel>().ToList();

            return this.PartialView("_ViewPostsPartial", posts);
        }
    }
}