namespace TopPost.Web.ViewModels.Comments
{
    using System;

    using AutoMapper;

    using TopPost.Models;
    using TopPost.Web.Infrastructure.Mapping;

    public class CommentDetailViewModel : IMapFrom<Comment>, IMapCustom
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, CommentDetailViewModel>()
                .ForMember(m => m.AuthorName, opt => opt.MapFrom(p => p.Author.UserName))
                .ForMember(m => m.CommentsCount, opt => opt.MapFrom(p => p.Comments.Count))
                .ForMember(m => m.LikesCount, opt => opt.MapFrom(p => p.Likes.Count))
                .ReverseMap();
        }
    }
}