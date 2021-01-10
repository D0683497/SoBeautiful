using AutoMapper;
using SoBeautiful.Dtos.Comment;
using SoBeautiful.Entities;

namespace SoBeautiful.Mappers
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            #region CreateDto 轉換成 Comment

            CreateMap<CreateDto, Comment>()
                .ForMember(dest => dest.CommentId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.ArticleId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Article,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.User,
                    opt => opt.Ignore());

            #endregion
            
            #region Comment 轉換成 SingleDto

            CreateMap<Comment, SingleDto>()
                .ForMember(dest => dest.CommentId,
                    opt => opt.MapFrom(src => src.CommentId))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content));

            #endregion
        }
    }
}