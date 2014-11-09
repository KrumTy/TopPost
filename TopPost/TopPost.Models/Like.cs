namespace TopPost.Models
{
    public class Like
    {
        public int Id { get; set; }

        public bool? Value { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int? CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public int? PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
