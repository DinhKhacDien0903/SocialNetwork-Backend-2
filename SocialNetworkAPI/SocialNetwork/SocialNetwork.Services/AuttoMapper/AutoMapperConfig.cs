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

            //image
            CreateMap<ImagesOfPostEntity, ImagesOfPostViewModel>().ReverseMap();

            //comment
            CreateMap<CommentEntity, CommentViewModel>().ReverseMap();
            CreateMap<CommentEntity, CommentRequest>().ReverseMap();
            CreateMap<CommentEntity, CommentRespone>().ReverseMap();
            CreateMap<CommentViewModel,CommentRequest>().ReverseMap();

            //reactionPost
            CreateMap<ReactionPostEntity, ReactionPostViewModel>().ReverseMap();
            CreateMap<ReactionPostEntity, ReactionRequest>().ReverseMap();

            //reaction
            CreateMap<ReactionEntity, ReactionRequest>().ReverseMap();
            CreateMap<ReactionPostViewModel, ReactionEntity>().ReverseMap();

            //xuoi

            CreateMap<UserEntity, UserViewModel>();
            CreateMap<MessagesEntity, MessageViewModel>();
            CreateMap<UserEntity, FriendViewModel>();
            CreateMap<MessagesEntity, MessagePersonResponse>();
            CreateMap<MessageImageEntity, MessageImageViewModel>();

            //nguoc lai
            CreateMap<UserViewModel, UserEntity>();
            CreateMap<MessageViewModel, MessagesEntity>();
            CreateMap<FriendViewModel, UserEntity>();
            CreateMap<MessagePersonResponse, MessagesEntity>();
            CreateMap<MessageImageViewModel, MessageImageEntity>();
        }
    }
}
