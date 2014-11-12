namespace TopPost.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TopPost.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TopPost.Data.TopPostDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
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

        protected override void Seed(TopPost.Data.TopPostDbContext context)
        {
            if (context.Comments.Any())
            {
                //StaticDataSeeder.SeedUsers(context);
                //StaticDataSeeder.SeedData(context);
                return;
            }

            Random Rand = new Random();

            List<Category> Categories;

            List<Post> Posts;

            Categories = new List<Category>();
            Categories.Add(new Category() { Name = "Art" });
            Categories.Add(new Category() { Name = "Economy" });
            Categories.Add(new Category() { Name = "Technology" });
            Categories.Add(new Category() { Name = "Education" });
            Categories.Add(new Category() { Name = "Sports" });
            Categories.Add(new Category() { Name = "Science" });
            Categories.Add(new Category() { Name = "Funny" });

            ApplicationUser user = new ApplicationUser() { UserName = "Anonimous" };

            Posts = new List<Post>();

            for (int j = 0; j < 10; j++)
            {
                for (int i = 1; i < 11; i++)
                {
                    Posts.Add(new Post()
                    {
                        Category = Categories[1],
                        Title = "Title " + (i + 10 * j),
                        Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry&amp;amp;amp;#39;s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                        Author = user,
                        Comments = GetSomeComments(user),
                        ImageUrl = "/images/" + i + ".jpg",
                        ThumbnailUrl = "/images/thumbnails/" + i + ".jpg"
                    });
                }
            }

            context.Posts.AddOrUpdate(Posts.ToArray());
        }
    }
}
