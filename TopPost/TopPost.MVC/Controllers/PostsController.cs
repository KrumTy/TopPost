using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopPost.Data;
using TopPost.Models;
using PagedList;
using TopPost.MVC.Models;
using System.Drawing;
using TopPost.ImageUpload;

namespace TopPost.MVC.Controllers
{
    public class PostsController : BaseController
    {
        // GET: Post
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? show)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var posts = db.Posts.All();

            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper())
                                       || s.Description.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    posts = posts.OrderBy(s => s.Title);
                    break;
                case "name_desc":
                    posts = posts.OrderByDescending(s => s.Title);
                    break;
                case "date_asc":
                    posts = posts.OrderBy(s => s.Created);
                    break;
                case "date_desc":
                    posts = posts.OrderByDescending(s => s.Created);
                    break;
                default:
                    posts = posts.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = show == null ? 3 : (int)show;
            int pageNumber = (page ?? 1);
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult All(SortingPagingInfo info)
        {
            if (info.SortField == null)
            {
                info = SortingPagingInfo.Default();
            }
            info.PageCount = Convert.ToInt32(Math.Ceiling((double)(db.Posts.All().Count() / info.PageSize)));

            IQueryable<Post> posts = null;
            switch (info.SortField)
            {
                case "Title":
                    posts = (info.SortDirection == "Ascending" ?
                    db.Posts.All().OrderBy(c => c.Title) :
                    db.Posts.All().OrderByDescending(c => c.Title));
                    break;
                case "Description":
                    posts = (info.SortDirection == "Ascending" ?
                    db.Posts.All().OrderBy(c => c.Description) :
                    db.Posts.All().OrderByDescending(c => c.Description));
                    break;
                case "Created":
                    posts = (info.SortDirection == "Ascending" ?
                    db.Posts.All().OrderBy(c => c.Created) :
                    db.Posts.All().OrderByDescending(c => c.Created));
                    break;
            }

            ViewBag.SortingPagingInfo = info;
            if (info.CurrentPageIndex * info.PageSize > posts.Count())
            {
                info.CurrentPageIndex = 0;
            }
            posts = posts.Skip(info.CurrentPageIndex * info.PageSize).Take(info.PageSize);
            List<Post> model = posts.ToList();

            return View(model);
        }

        public ActionResult Show(int id)
        {
            var post = this.db.Posts.Find(id);

            return View(post); // TODO: Models
        }

        public ActionResult FileUpload(Post post, HttpPostedFileBase file)
        {
            if (file != null && ModelState.IsValid)
            {
                var urls = ImageUploader.UrlFromFile(file);
                post.ImageUrl = urls[0];
                post.ThumbnailUrl = urls[1];
                post.CategoryId = 1;

                db.Posts.Add(post);
                db.SaveChanges();
            }

            return RedirectToAction("../home/Display/");
        }
    }
}