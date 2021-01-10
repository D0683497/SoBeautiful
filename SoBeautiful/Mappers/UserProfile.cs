using AutoMapper;
using SoBeautiful.Dtos.Account;
using SoBeautiful.Entities;

namespace SoBeautiful.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region RegisterDto 傳換成 ApplicationUser
            
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.NormalizedUserName,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedEmail,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp,
                    opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed,
                    opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled,
                    opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd,
                    opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled,
                    opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsEnable,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Surname,
                    opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.GivenName,
                    opt => opt.MapFrom(src => src.GivenName))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Articles,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Comments,
                    opt => opt.Ignore());
            
            #endregion
        }
    }
}