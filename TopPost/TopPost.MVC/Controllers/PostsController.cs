﻿using System;
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

            var posts = db.Posts.All();

            // Filter
            if (info.Filter != null)
            {
                if (info.FilterBy == "Title")
                {
                    posts = posts.Where(p => p.Title.Contains(info.Filter));
                }
                else if (info.FilterBy == "Tag")
                {
                    var tag = this.db.Tags.All().FirstOrDefault(t => t.Name == info.Filter);

                    if (tag == null)
                    {
                        info.PageCount = 0;
                        info.CurrentPageIndex = 0;
                        ViewBag.SortingPagingInfo = info;
                        return View();
                    }
                    else
                    {
                        posts = tag.Posts.AsQueryable();
                    }
                }
            }

            info.PageCount = (posts.Count() / info.PageSize);

            
            // Sort
            switch (info.SortField)
            {
                case "Title":
                    posts = (info.SortDirection == "Ascending" ?
                    posts.OrderBy(c => c.Title) :
                    posts.OrderByDescending(c => c.Title));
                    break;
                case "Description":
                    posts = (info.SortDirection == "Ascending" ?
                    posts.OrderBy(p => p.Description) :
                    posts.OrderByDescending(c => c.Description));
                    break;
                case "Created":
                    posts = (info.SortDirection == "Ascending" ?
                    posts.OrderBy(p => p.Created) :
                    posts.OrderByDescending(c => c.Created));
                    break;
                case "Comments":
                    posts = (info.SortDirection == "Ascending" ?
                    posts.OrderBy(p => p.Comments.Count) :
                    posts.OrderByDescending(c => c.Comments.Count));
                    break;
                case "Favorites":
                    posts = (info.SortDirection == "Ascending" ?
                    posts.OrderBy(p => p.Favorites.Count) :
                    posts.OrderByDescending(c => c.Favorites.Count));
                    break;
                case "Likes":
                    posts = (info.SortDirection == "Ascending" ?
                    posts.OrderBy(p => p.Likes.Where(l => l.Value == true).Count() - p.Likes.Where(l => l.Value == false).Count()) :
                    posts.OrderByDescending(p => p.Likes.Where(l => l.Value == true).Count() - p.Likes.Where(l => l.Value == false).Count()));
                    break;
            }

            if (info.CurrentPageIndex * info.PageSize > posts.Count())
            {
                info.CurrentPageIndex = 0;
            }

            ViewBag.SortingPagingInfo = info;

            posts = posts.Skip(info.CurrentPageIndex * info.PageSize).Take(info.PageSize);
            List<Post> model = posts.ToList();

            return View(model);
        }

        public ActionResult Show(int id)
        {
            var post = this.db.Posts.Find(id);

            return View(post); // TODO: Models
        }

        public ActionResult GetUserPosts(string username)
        {
            var user = this.db.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                user = this.GetUser();
            }

            var posts = user.Posts.ToList();

            return PartialView("_ViewPostsPartial", posts);
        }

        public ActionResult Post(Post post, HttpPostedFileBase file, string StringTags)
        {
            if (file != null && ModelState.IsValid)
            {
                var urls = ImageUploader.UrlFromFile(file);
                post.ImageUrl = urls[0];
                post.ThumbnailUrl = urls[1];
                post.CategoryId = 1;

                var tags = StringTags.Split(',').Select(x => new Tag() { Name = x.Trim() }).ToArray();

                foreach (var tag in tags)
                {
                    var existigTag = this.db.Tags.All().FirstOrDefault(t => t.Name == tag.Name);
                    if (existigTag == null)
                    {
                        post.Tags.Add(tag);
                    }
                    else
                    {
                        post.Tags.Add(existigTag);
                    }
                }

                var user = this.GetUser();
                user.Posts.Add(post);
                db.Users.Update(user);

                db.SaveChanges();

                return RedirectToAction("Show", "Posts", new { id = post.Id });
            }

            return RedirectToAction("All", "Posts");
        }
    }
}