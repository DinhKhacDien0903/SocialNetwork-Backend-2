using SocialNetwork.DTOs.Response;
using SocialNetwork.DTOs.ViewModels;

namespace SocialNetwork.Services.AuttoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap<UserEntity, UserViewModel>();
            //CreateMap<UserViewModel, UserEntity>();

            CreateMap<PostEntity, PostViewModel>().ReverseMap();
            CreateMap<PostEntity, PostRequest>().ReverseMap();
            //CreateMap<PostViewModel,PostRequest>.ReverseMap();
            
            CreateMap<ImagesOfPostEntity, ImagesOfPostViewModel>().ReverseMap();

            CreateMap<PostViewModel, PostEntity>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<ImagesOfPostViewModel, ImagesOfPostEntity>();
            CreateMap<CommentViewModel,CommentEntity>().ReverseMap();

            //xuoi

            CreateMap<UserEntity, UserViewModel>();
            CreateMap<MessagesEntity, MessageViewModel>();
            CreateMap<UserEntity, FriendViewModel>();
            CreateMap<MessagesEntity, MessagePersonResponse>();

            //nguoc lai
            CreateMap<UserViewModel, UserEntity>();
            CreateMap<MessageViewModel, MessagesEntity>();
            CreateMap<FriendViewModel, UserEntity>();
            CreateMap<MessagePersonResponse, MessagesEntity>();

        }
    }
}
