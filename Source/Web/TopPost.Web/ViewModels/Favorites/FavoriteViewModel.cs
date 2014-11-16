namespace TopPost.Web.ViewModels.Favorites
{
    using TopPost.Models;
    using TopPost.Web.Infrastructure.Mapping;

    public class FavoriteViewModel : IMapFrom<Favorite>
    {
        public int Id { get; set; }

        public int PostId { get; set; }
    }
}