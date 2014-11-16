namespace TopPost.Web.ViewModels.Vote
{
    using AutoMapper;

    using TopPost.Models;
    using TopPost.Web.Infrastructure.Mapping;

    public class VoteViewModel : IMapFrom<Like>, IMapCustom
    {
        public int ContentId { get; set; }

        public bool? Value { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Like, VoteViewModel>()
                .ForMember(m => m.Value, opt => opt.MapFrom(p => p.Value))
                .ForMember(m => m.ContentId, opt => opt.MapFrom(p => p.PostId ?? p.CommentId))
                .ReverseMap();
        }
    }
}