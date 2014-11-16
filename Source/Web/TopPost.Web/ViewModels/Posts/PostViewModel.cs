namespace TopPost.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using TopPost.Models;
    using TopPost.Web.Infrastructure.Mapping;

    public class PostViewModel : IMapFrom<Post>, IMapCustom
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime Created { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public string CategoryName { get; set; }

        public string AuthorName { get; set; }

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        public int FavoritesCount { get; set; }

        public void CreateMappings(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(m => m.CategoryName, opt => opt.MapFrom(p => p.Category.Name))
                .ForMember(m => m.AuthorName, opt => opt.MapFrom(p => p.Author.UserName))
                .ForMember(m => m.CommentsCount, opt => opt.MapFrom(p => p.Comments.Count))
                .ForMember(m => m.LikesCount, opt => opt.MapFrom(p => p.Likes.Count))
                .ForMember(m => m.FavoritesCount, opt => opt.MapFrom(p => p.Favorites.Count))
                .ReverseMap();
        }
    }
}