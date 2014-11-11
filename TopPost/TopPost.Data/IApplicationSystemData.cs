namespace TopPost.Data
{
    using TopPost.Data.Repositories;
    using TopPost.Models;

    public interface IApplicationSystemData
    {
        IRepository<Post> Posts { get; }
        IRepository<Category> Categories { get; }
        IRepository<ApplicationUser> Users { get; }
        int SaveChanges();
    }
}
