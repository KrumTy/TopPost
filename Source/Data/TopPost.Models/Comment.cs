namespace TopPost.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using TopPost.Data.Common.Models;

    public class Comment : DeletableEntity
    {
        private ICollection<Like> likes;
        private ICollection<Comment> comments;

        public Comment()
        {
            this.likes = new HashSet<Like>();
            this.comments = new HashSet<Comment>();

            this.Created = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Text Field")]
        public string Text { get; set; }

        public DateTime Created { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public int? ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<Like> Likes
        {
            get { return this.likes; }
            set { this.likes = value; }
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }
    }
}
