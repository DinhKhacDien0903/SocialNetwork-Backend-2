using Azure.Core;
using SocialNetwork.Services.Unit;

public class CommentService : ICommentService
{
    private readonly ICommentRepositories _commentRepositories;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;


    public CommentService(ICommentRepositories commentRepositories, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _commentRepositories = commentRepositories;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CommentViewModel> AddCommentAsync(CommentRequest commentRequest)
    {
        if (string.IsNullOrEmpty(commentRequest.Content))
            throw new ArgumentException("Content cannot be empty.");
        

        var commentEntity = _mapper.Map<CommentEntity>(commentRequest);
        commentEntity.CommentID = Guid.NewGuid().ToString();
        await _commentRepositories.AddCommentAsync(commentEntity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CommentViewModel>(commentRequest);
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

    public async Task<IEnumerable<CommentViewModel>> GetCommentByPostIdAsync(string postId)
    {
        var comments = await _commentRepositories.GetCommentsByPostIdAsync(postId);
        return _mapper.Map<IEnumerable<CommentViewModel>>(comments);
    }

    public async Task<IEnumerable<CommentViewModel>> GetRepliesByCommentIdAsync(string parentCommentId)
    {
        var replies = await _commentRepositories.GetRepliesByCommentIdAsync(parentCommentId);
        return _mapper.Map<IEnumerable<CommentViewModel>>(replies);
    }

    public async Task UpdateCommentAsync(CommentViewModel commentViewModel)
    {
        var comment = await _commentRepositories.GetCommentByIdAsync(commentViewModel.CommentID);
        if (comment == null)
            throw new KeyNotFoundException("Comment not found.");
         _mapper.Map(commentViewModel, comment);
        await _commentRepositories.UpdateCommentAsync(comment);
    }
}
