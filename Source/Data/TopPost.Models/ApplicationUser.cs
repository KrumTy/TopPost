namespace TopPost.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using TopPost.Data.Common.Models;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        private ICollection<Post> posts;
        private ICollection<Comment> comments;
        private ICollection<Like> likes;
        private ICollection<Favorite> favorites;


        public ApplicationUser()
            : base()
        {
            this.CreatedOn = DateTime.Now;

            this.posts = new HashSet<Post>();
            this.comments = new HashSet<Comment>();
            this.likes = new HashSet<Like>();
            this.favorites = new HashSet<Favorite>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<Post> Posts
        {
            get { return this.posts; }
            set { this.posts = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Like> Likes
        {
            get { return this.likes; }
            set { this.likes = value; }
        }

        public virtual ICollection<Favorite> Favorites
        {
            get { return this.favorites; }
            set { this.favorites = value; }
        }

        // IAuditInfo
        public DateTime CreatedOn { get; set; }

        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // IDeletableEntity
        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
