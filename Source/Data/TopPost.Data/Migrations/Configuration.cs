namespace TopPost.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TopPost.Models;
    using TopPost.Common;

    internal sealed class Configuration : DbMigrationsConfiguration<TopPostDbContext>
    {
        private UserManager<ApplicationUser> userManager;

        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TopPost.Data.TopPostDbContext context)
        {
            if (context.Comments.Any())
            {
                return;
            }

            AddAdmin(context);

            Random rand = new Random();

            List<Post> posts;

            List<Category> categories = new List<Category>();
            categories.Add(new Category() { Name = "Art" });
            categories.Add(new Category() { Name = "Economy" });
            categories.Add(new Category() { Name = "Technology" });
            categories.Add(new Category() { Name = "Education" });
            categories.Add(new Category() { Name = "Sports" });
            categories.Add(new Category() { Name = "Science" });
            categories.Add(new Category() { Name = "Funny" });

            //context.Categories.AddOrUpdate(categories.ToArray());
            //context.SaveChanges();

            ApplicationUser user = new ApplicationUser() { UserName = "Anonimous" }; // this.context.Users.Fir

            posts = new List<Post>();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 1; i < 11; i++)
                {
                    posts.Add(new Post()
                    {
                        Category = categories[1],
                        Title = "Title " + (i + (10 * j)),
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry&amp.",
                        Author = user,
                        Comments = GetSomeComments(user),
                        ImageUrl = "/images/" + i + ".jpg",
                        ThumbnailUrl = "/images/thumbnails/" + i + ".jpg"
                    });
                }
            }

            context.Posts.AddOrUpdate(posts.ToArray());
        }

        private void AddAdmin(TopPost.Data.TopPostDbContext context)
        {
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            context.Roles.AddOrUpdate(x => x.Name, new IdentityRole(GlobalConstants.AdminRole));
            context.SaveChanges();

            var adminUser = new ApplicationUser
            {
                Email = "admin@gmail.com",
                UserName = "admin"
            };

            this.userManager.Create(adminUser, "123456");
            this.userManager.AddToRole(adminUser.Id, GlobalConstants.AdminRole);
        }

        private static List<Comment> GetSomeComments(ApplicationUser user)
        {
            return new List<Comment>()
            {
                new Comment()
                {
                    Author = user,
                    Text = "Nice"
                },
                new Comment()
                {
                    Author = user,
                    Text = "Funny"
                },
                new Comment()
                {
                    Author = user,
                    Text = "LoL"
                }
            };
        }
    }
}
