using AutoMapper;
using SoBeautiful.Dtos.Article;
using SoBeautiful.Entities;

namespace SoBeautiful.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            #region CreateDto 轉換成 Article

            CreateMap<CreateDto, Article>()
                .ForMember(dest => dest.ArticleId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Like,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Dislike,
                    opt => opt.Ignore())
                .ForPath(dest => dest.Comments,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.User,
                    opt => opt.Ignore());

            #endregion

            #region Article 轉換成 SingleDto

            CreateMap<Article, SingleDto>()
                .ForMember(dest => dest.ArticleId,
                    opt => opt.MapFrom(src => src.ArticleId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Like,
                    opt => opt.MapFrom(src => src.Like))
                .ForMember(dest => dest.Dislike,
                    opt => opt.MapFrom(src => src.Dislike));

            #endregion
        }
    }
}