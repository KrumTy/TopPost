namespace TopPost.Data
{
    using TopPost.Data.Common.Repositories;
    using TopPost.Models;

    public interface ITopPostData
    {
        IDeletableEntityRepository<Post> Posts { get; }
        IDeletableEntityRepository<Category> Categories { get; }
        IDeletableEntityRepository<ApplicationUser> Users { get; }
        int SaveChanges();
    }
}
