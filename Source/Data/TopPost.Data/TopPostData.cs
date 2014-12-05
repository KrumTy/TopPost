namespace TopPost.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using TopPost.Data.Common.Models;
    using TopPost.Data.Common.Repositories;
    using TopPost.Models;

    public class TopPostData : ITopPostData
    {
        private ITopPostDbContext context;
        private IDictionary<Type, object> repositories;

        public TopPostData()
            : this(new TopPostDbContext())
        {
        }

        public TopPostData(TopPostDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public ITopPostDbContext Context
        {
            get
            {
                return this.context;
            }
        }

        public IDeletableEntityRepository<Post> Posts
        {
            get { return this.GetRepository<Post>(); }
        }

        public IDeletableEntityRepository<Category> Categories
        {
            get { return this.GetRepository<Category>(); }
        }

        public IDeletableEntityRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IDeletableEntityRepository<Like> Likes
        {
            get { return this.GetRepository<Like>(); }
        }

        public IDeletableEntityRepository<Favorite> Favorites
        {
            get { return this.GetRepository<Favorite>(); }
        }

        public IDeletableEntityRepository<Tag> Tags
        {
            get { return this.GetRepository<Tag>(); }
        }

        public IDeletableEntityRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public static TopPostData Create()
        {
            return new TopPostData();
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IDeletableEntityRepository<T> GetRepository<T>() where T : class, IDeletableEntity
        {
            var typeOfModel = typeof(T);

            if (!this.repositories.ContainsKey(typeOfModel))
            {
                this.repositories.Add(typeOfModel, Activator.CreateInstance(typeof(DeletableEntityRepository<T>), this.context));
            }

            return (IDeletableEntityRepository<T>)this.repositories[typeOfModel];
        }
    }
}
