namespace TopPost.Web.ViewModels.Posts
{
    using TopPost.Models;
    using TopPost.Web.Infrastructure.Mapping;

    public class PostThumbViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }
    }
}