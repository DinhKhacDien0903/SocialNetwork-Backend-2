using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Helpers.Hubs;
using SocialNetwork.Services.Unit;

public class CommentService : ICommentService
{
    private readonly ICommentRepositories _commentRepositories;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    //private readonly IHubContext<PostHub> _hubContext;
    private readonly IPostHubService _postHubService;


    public CommentService(ICommentRepositories commentRepositories, IMapper mapper, IUserRepository userRepository,  IPostHubService postHubService)
    {
        _commentRepositories = commentRepositories;
        _mapper = mapper;
        _userRepository = userRepository;
        //_hubContext = hubContext;
        _postHubService = postHubService;
    }

    public async Task<CommentViewModel> AddCommentAsync(CommentRequest commentRequest, string userId)
    {
        if (string.IsNullOrEmpty(commentRequest.Content))
            throw new ArgumentException("Content cannot be empty.");


        var commentEntity = _mapper.Map<CommentEntity>(commentRequest);

        commentEntity.CommentID = Guid.NewGuid().ToString();

        commentEntity.UserID = userId;

        await _commentRepositories.AddCommentAsync(commentEntity);

        var user = await _userRepository.GetByIDAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found.");
        }


        var comment = _mapper.Map<CommentViewModel>(commentEntity);

        comment.LastName = user?.LastName;
        comment.FirstName=user?.FirstName;
        int count = await _commentRepositories.GetCountComment(commentEntity.PostID);
        int number = count + 1;


        await _postHubService.SendCommentAsycn(comment, number);
        return comment;

       

    }


    public async Task DeleteCommentAsync(string commentId)
    {
        await _commentRepositories.DeleteCommentAsync(commentId);
    }


    public async Task<IEnumerable<CommentViewModel>> GetAllCommentAsync()
    {
        var comments = await _commentRepositories.GetAllAsync();
        return _mapper.Map<IEnumerable<CommentViewModel>>(comments);
    }

    public async Task<CommentViewModel> GetCommentByIdAsync(string commentId)
    {
        var comment = await _commentRepositories.GetCommentByIdAsync(commentId);
        if (comment == null)
            throw new KeyNotFoundException("Comment not found.");

        return _mapper.Map<CommentViewModel>(comment);
    }

    public async Task<CommentResultViewModel> GetCommentByPostIdAsync(string postId)
    {
        var comments = await _commentRepositories.GetCommentsByPostIdAsync(postId);

        return new CommentResultViewModel
        {
            Comment = comments.Comment,
            NumberOfComment = comments.NumberOfComment,

        };
    }

    public async Task<int> GetCommentCountByPostIdAsync(string postId)
    {
        return await _commentRepositories.GetCommentCountByPostIdAsync(postId);
    }

    //public async Task<IEnumerable<CommentViewModel>> GetRepliesByCommentIdAsync(string parentCommentId)
    //{
    //    var replies = await _commentRepositories.GetRepliesByCommentIdAsync(parentCommentId);
    //    return _mapper.Map<IEnumerable<CommentViewModel>>(replies);
    //}

    public async Task UpdateCommentAsync(CommentViewModel commentViewModel)
    {
        var comment = await _commentRepositories.GetCommentByIdAsync(commentViewModel.CommentID);
        if (comment == null)
            throw new KeyNotFoundException("Comment not found.");
        _mapper.Map(commentViewModel, comment);
        await _commentRepositories.UpdateCommentAsync(comment);
    }
}
