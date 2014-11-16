namespace TopPost.Models
{
    using System.ComponentModel.DataAnnotations;

    using TopPost.Data.Common.Models;

    public class Favorite : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
