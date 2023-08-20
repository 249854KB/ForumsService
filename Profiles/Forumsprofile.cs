using AutoMapper;
using ForumsService.Models;
using ForumsService.Dtos;
using UserService;

namespace ForumsService.Profiles
{
    public class ForumsProfile : Profile
    {
        public ForumsProfile()
        {
            //Source -> target
            CreateMap<User, UserReadDto>();
            CreateMap<ForumCreateDto, Forum>();
            CreateMap<Forum, ForumReadDto>();
            CreateMap<ForumReadDto, ForumPublishedDto>();
            CreateMap<UserPublishedDto, User>()
                .ForMember(destination =>destination.ExternalID, opt => opt.MapFrom(source => source.Id));
            CreateMap<GrpcUserModel, User>()
                .ForMember(destination => destination.ExternalID, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Forums, opt =>opt.Ignore());
            CreateMap<Forum,GrpcForumModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src=>src.Id))
                .ForMember(dest => dest.Title,opt => opt.MapFrom(src=>src.Title))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src=>src.Text))
                .ForMember(dest => dest.DogId, opt => opt.MapFrom(src=>src.DogId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src=>src.UserId));;

        }
    }
}