namespace TopPost.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using PagedList;

    using TopPost.Data;
    using TopPost.Data.Common;
    using TopPost.Data.Common.Repositories;
    using TopPost.ImageUpload;
    using TopPost.Models;
    using TopPost.Web.Models;
    using TopPost.Web.ViewModels.Posts;
    using TopPost.Web.Infrastructure.Caching;

    public class PostsController : BaseController
    {
        private readonly IDeletableEntityRepository<Post> posts;
        private ICacheService cache;

        public PostsController(ITopPostData data, IDeletableEntityRepository<Post> posts, ICacheService cache)
            : base(data)
        {
            this.posts = posts;
            this.cache = cache;
        }

        [OutputCache(Duration = 60)]
        public ActionResult Load(int skip = 0, int take = 5)
        {
            string type = this.TempData["type"].ToString();

            var date = DateTime.Now.AddDays(-1);
            int neededVotes = -1;
            List<PostViewModel> nextPosts;

            if (type == "Front")
            {
                neededVotes = 0;
            }
            else if (type == "Rising")
            {
                neededVotes = 0;
            }
            else if (type == "New")
            {
                neededVotes = -10;
            }

            if (type == "Top")
            {
                nextPosts = this.posts
                .All()
                .OrderByDescending(p => p.Likes.Where(l => l.Value == true && !l.IsDeleted).Count() - p.Likes.Where(l => l.Value == false && !l.IsDeleted).Count())
                .Skip(skip)
                .Take(take)
                .Project()
                .To<PostViewModel>()
                .ToList();
            }
            else
            {
                nextPosts = this.posts
                  .All()
                  //.Where(x => DateTime.Compare(x.Created, date) > 0 && !x.IsDeleted)
                  .Where(p => p.Likes.Where(l => l.Value == true && !l.IsDeleted).Count() - p.Likes.Where(l => l.Value == false && !l.IsDeleted).Count() > neededVotes)
                  .OrderByDescending(x => x.Created)
                  .Skip(skip)
                  .Take(take)
                  .Project()
                  .To<PostViewModel>()
                  .ToList();
            }

            this.ViewBag.type = type;
            this.ViewBag.PostsCount = skip + take;
            this.TempData.Remove("type");
            this.TempData.Add("type", type);

            return this.PartialView("_LoadNextPostsPartial", nextPosts);
        }

        public ActionResult Front(string type = "Front")
        {
            this.ViewBag.lastPostId = 0;
            this.TempData.Remove("type");
            this.TempData.Add("type", type);

            return this.View();
        }

        // GET: Post
        //[ChildActionOnly]
        [OutputCache(Duration = 60 * 60)]
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

            var displayPosts = this.posts.All(); // db.Posts.All();

            if (!string.IsNullOrEmpty(searchString))
            {
                displayPosts = displayPosts.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper())
                                       || s.Description.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    displayPosts = displayPosts.OrderBy(s => s.Title);
                    break;
                case "name_desc":
                    displayPosts = displayPosts.OrderByDescending(s => s.Title);
                    break;
                case "date_asc":
                    displayPosts = displayPosts.OrderBy(s => s.Created);
                    break;
                case "date_desc":
                    displayPosts = displayPosts.OrderByDescending(s => s.Created);
                    break;
                default:
                    displayPosts = displayPosts.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = show == null ? 3 : (int)show;
            int pageNumber = page ?? 1;

            return this.View(displayPosts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult All(SortingPagingInfo info)
        {
            if (info.SortField == null)
            {
                info = SortingPagingInfo.Default();
            }

            var displayPosts = GetAllPosts();// db.Posts.All();

            // Filter
            if (info.Filter != null)
            {
                if (info.FilterBy == "Title")
                {
                    displayPosts = displayPosts.Where(p => p.Title.Contains(info.Filter));
                }
                else if (info.FilterBy == "Tag")
                {
                    var tag = this.db.Tags.All().FirstOrDefault(t => t.Name == info.Filter);

                    if (tag == null)
                    {
                        info.PageCount = 0;
                        info.CurrentPageIndex = 0;
                        ViewBag.SortingPagingInfo = info;
                        return this.View();
                    }
                    else
                    {
                        displayPosts = tag.Posts.AsQueryable();
                    }
                }
            }

            info.PageCount = displayPosts.Count() / info.PageSize;

            // Sort
            switch (info.SortField)
            {
                case "Title":
                    displayPosts = info.SortDirection == "Ascending" ?
                    displayPosts.OrderBy(c => c.Title) :
                    displayPosts.OrderByDescending(c => c.Title);
                    break;
                case "Description":
                    displayPosts = info.SortDirection == "Ascending" ?
                    displayPosts.OrderBy(p => p.Description) :
                    displayPosts.OrderByDescending(c => c.Description);
                    break;
                case "Created":
                    displayPosts = info.SortDirection == "Ascending" ?
                    displayPosts.OrderBy(p => p.Created) :
                    displayPosts.OrderByDescending(c => c.Created);
                    break;
                case "Comments":
                    displayPosts = info.SortDirection == "Ascending" ?
                    displayPosts.OrderBy(p => p.Comments.Count) :
                    displayPosts.OrderByDescending(c => c.Comments.Count);
                    break;
                case "Favorites":
                    displayPosts = info.SortDirection == "Ascending" ?
                    displayPosts.OrderBy(p => p.Favorites.Count) :
                    displayPosts.OrderByDescending(c => c.Favorites.Count);
                    break;
                case "Likes":
                    displayPosts = info.SortDirection == "Ascending" ?
                    displayPosts.OrderBy(p => p.Likes.Where(l => l.Value == true && !l.IsDeleted).Count() - p.Likes.Where(l => l.Value == false && !l.IsDeleted).Count()) :
                    displayPosts.OrderByDescending(p => p.Likes.Where(l => l.Value == true && !l.IsDeleted).Count() - p.Likes.Where(l => l.Value == false && !l.IsDeleted).Count());
                    break;
            }

            if (info.CurrentPageIndex * info.PageSize > displayPosts.Count())
            {
                info.CurrentPageIndex = 0;
            }

            ViewBag.SortingPagingInfo = info;

            displayPosts = displayPosts.Skip(info.CurrentPageIndex * info.PageSize).Take(info.PageSize);
            var model = displayPosts
                .Project()
                .To<PostThumbViewModel>()
                .ToList();

            return this.View(model);
        }

        private IQueryable<Post> GetAllPosts()
        {
            var displayPosts = this.cache.Get<IQueryable<Post>>("posts",
                () =>
                {
                    return this.db.Posts
                       .All();
                    //.Select(c => new SelectListItem
                    //{
                    //    Value = c.Id.ToString(),
                    //    Text = c.Name
                    //})
                    //.ToList();
                }); //this.posts.All();

            return displayPosts;
        }

        public ActionResult Show(int id)
        {
            var post = this.posts.Find(id); // this.db.Posts.Find(id);
            var postView = Mapper.Map<PostViewModel>(post);

            return this.View(postView); // TODO: Models
        }

        public ActionResult GetUserPosts(string username)
        {
            var user = this.db.Users.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                user = this.GetUser();
            }

            var posts = user.Posts.AsQueryable().Where(p => !p.IsDeleted).Project().To<PostThumbViewModel>().ToList();

            return this.PartialView("_ViewPostsPartial", posts);
        }

        [ValidateInput(false)] 
        public ActionResult Post(PostInputModel post)
        {
            if (post.file != null && ModelState.IsValid)
            {
                var urls = ImageUploader.UrlFromFile(post.file);

                var newPost = new Post()
                {
                    Title = post.Title,
                    Description = post.Description,
                    ImageUrl = urls[0],
                    ThumbnailUrl = urls[1],
                    CategoryId = 1
                };

                var tags = post.StringTags.Split(',').Select(x => new Tag() { Name = x.Trim() }).ToArray();

                foreach (var tag in tags)
                {
                    var existigTag = this.db.Tags.All().FirstOrDefault(t => t.Name == tag.Name);
                    if (existigTag == null)
                    {
                        newPost.Tags.Add(tag);
                    }
                    else
                    {
                        newPost.Tags.Add(existigTag);
                    }
                }

                newPost.AuthorId = this.GetUser().Id;
                this.posts.Add(newPost);
                this.posts.SaveChanges();

                return this.RedirectToAction("Show", "Posts", new { id = newPost.Id });
            }

            return this.View("_PostPartial", post);
        }

        public ActionResult GetPostPartial()
        {
            return this.PartialView("_PostPartial");
        }

        public ActionResult GetTopDailyPosts(int take = 40)
        {
            var date = DateTime.Now.AddDays(-1);

            return this.PartialView("_TopDailyPosts",
                this.posts.All()
                .AsQueryable()
                .Where(x => DateTime.Compare(x.Created, date) > 0 && !x.IsDeleted)
                .OrderByDescending(p => p.Likes
                    .Where(l => l.Value == true && !l.IsDeleted)
                    .Count() - p.Likes
                    .Where(l => l.Value == false && !l.IsDeleted)
                    .Count()).Take(take)
                .Project()
                .To<PostThumbViewModel>()
                .ToList());
        }

        public ActionResult Delete(int postId)
        {
            var post = this.db.Posts.Find(postId);
            var user = this.UserProfile;

            if (post == null || user.Id != post.AuthorId)
            {
                return this.RedirectToAction("Show", "Posts", new { id = postId });
            }

            foreach (var comment in post.Comments)
            {
                this.db.Comments.Delete(comment);
            }

            foreach (var fav in post.Favorites)
            {
                this.db.Favorites.Delete(fav);
            }

            this.db.Posts.Delete(post.Id);

            this.db.SaveChanges();

            return RedirectToAction("Front");
        }
    }
}