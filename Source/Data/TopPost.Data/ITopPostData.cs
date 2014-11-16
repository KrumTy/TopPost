namespace TopPost.Data
{
    using TopPost.Data.Common.Repositories;
    using TopPost.Models;

    public interface ITopPostData
    {
        IDeletableEntityRepository<Post> Posts { get; }

        IDeletableEntityRepository<Category> Categories { get; }

        IDeletableEntityRepository<ApplicationUser> Users { get; }
        IDeletableEntityRepository<Comment> Comments { get; }

        IDeletableEntityRepository<Favorite> Favorites { get; }

        IDeletableEntityRepository<Like> Likes { get; }

        IDeletableEntityRepository<Tag> Tags { get; }

        int SaveChanges();
    }
}
