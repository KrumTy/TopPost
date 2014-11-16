namespace TopPost.Models
{
    using System.ComponentModel.DataAnnotations;

    using TopPost.Data.Common.Models;

    public class Like : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public bool? Value { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public int? PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
