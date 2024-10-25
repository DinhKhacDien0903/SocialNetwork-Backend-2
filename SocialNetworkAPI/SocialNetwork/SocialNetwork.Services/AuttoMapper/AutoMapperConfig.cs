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

            //post
            CreateMap<PostEntity, PostViewModel>().ReverseMap();
            CreateMap<PostEntity, PostRequest>().ReverseMap();
            //        CreateMap<PostEntity, PostRequest>()
            //.ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
            //.ReverseMap();

            //image
            CreateMap<ImagesOfPostEntity, ImagesOfPostViewModel>().ReverseMap();

            //comment
            CreateMap<CommentEntity, CommentViewModel>().ReverseMap();
            CreateMap<CommentEntity, CommentRequest>().ReverseMap();
            CreateMap<CommentEntity, CommentRespone>().ReverseMap();

            //reactionPost
            CreateMap<ReactionPostEntity, ReactionPostViewModel>().ReverseMap();
            CreateMap<ReactionPostEntity, ReactionRequest>().ReverseMap();

            //reaction
            CreateMap<ReactionEntity, ReactionRequest>().ReverseMap();
            CreateMap<ReactionPostViewModel, ReactionEntity>().ReverseMap();

            //xuoi

            CreateMap<UserEntity, UserViewModel>();
            CreateMap<MessagesEntity, MessageViewModel>();


            //nguoc lai
            CreateMap<UserViewModel, UserEntity>();
            CreateMap<MessageViewModel, MessagesEntity>();

        }
    }
}
