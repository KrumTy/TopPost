namespace TopPost.Web.Infrastructure.Mapping
{
    using AutoMapper;

    public interface IMapCustom
    {
        void CreateMappings(IConfiguration configuration);
    }
}