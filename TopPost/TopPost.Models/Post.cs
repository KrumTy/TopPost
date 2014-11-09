namespace TopPost.Models
{
    using System;
    using System.Collections.Generic;

    public class Post : IContent
    {
        private ICollection<Comment> comments;
        private ICollection<Like> likes;

        public Post()
        {
            this.comments = new HashSet<Comment>();
            this.likes = new HashSet<Like>();

            this.Created = DateTime.Now;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime Created { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

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
    }
}
