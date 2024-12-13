using SocialNetwork.DataAccess.Repositories;
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
            CreateMap<PostEntity, PostResponse>().ReverseMap();

            //image
            CreateMap<ImagesOfPostEntity, ImagesOfPostViewModel>().ReverseMap();

            //comment
            CreateMap<CommentEntity, CommentViewModel>().ReverseMap();
            CreateMap<CommentEntity, CommentRequest>().ReverseMap();
            CreateMap<CommentEntity, CommentViewModel>().ReverseMap();
            CreateMap<CommentViewModel,CommentRequest>().ReverseMap();
            CreateMap<CommentViewModel, CommentViewModel>().ReverseMap();


            //reactionPost
            CreateMap<ReactionPostEntity, ReactionPostViewModel>().ReverseMap();
            CreateMap<ReactionPostEntity, ReactionRequest>().ReverseMap();
            CreateMap<EmotionRequest,EmotionTypeEntity>().ReverseMap();

            //reaction
            CreateMap<ReactionEntity, ReactionRequest>().ReverseMap();
            CreateMap<ReactionPostViewModel, ReactionEntity>().ReverseMap();
            CreateMap<ReactionRepository,ReactionPostEntity>().ReverseMap();

            //User
            CreateMap<UserEntity, UserSearchViewModel>().ReverseMap();

            //relationship
            CreateMap<UserSearchViewModel,UserEntity>().ReverseMap();
            CreateMap<UserSearchViewModel,RelationshipEntity>().ReverseMap();


            //xuoi

            CreateMap<UserEntity, UserViewModel>();
            CreateMap<MessagesEntity, MessageViewModel>();
            CreateMap<UserEntity, FriendViewModel>();
            CreateMap<MessagesEntity, MessagePersonResponse>();
            CreateMap<MessageImageEntity, MessageImageViewModel>();
            CreateMap<GroupChatEntity, GroupChatViewModel>();
            CreateMap<NotificationEntity, NotificationViewModel>();

            //nguoc lai
            CreateMap<UserViewModel, UserEntity>();
            CreateMap<MessageViewModel, MessagesEntity>();
            CreateMap<FriendViewModel, UserEntity>();
            CreateMap<MessagePersonResponse, MessagesEntity>();
            CreateMap<MessageImageViewModel, MessageImageEntity>();
            CreateMap<GroupChatViewModel, GroupChatEntity>();
            CreateMap<NotificationViewModel, NotificationEntity>();

        }
    }
}
