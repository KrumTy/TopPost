namespace TopPost.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using TopPost.Models;

    public interface ITopPostDbContext
    {
        IDbSet<Category> Categories { get; set; }

        IDbSet<Comment> Comments { get; set; }

        IDbSet<Favorite> Favorites { get; set; }

        IDbSet<Like> Likes { get; set; }

        IDbSet<Post> Posts { get; set; }

        IDbSet<Tag> Tags { get; set; }

        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void Dispose();

        int SaveChanges();
    }
}
