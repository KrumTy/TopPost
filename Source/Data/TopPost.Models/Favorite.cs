using TopPost.Data.Common.Models;
namespace TopPost.Models
{
    public class Favorite : DeletableEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
