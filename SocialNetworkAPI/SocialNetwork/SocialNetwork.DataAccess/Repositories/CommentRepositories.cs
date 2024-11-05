using SocialNetwork.DataAccess.Repositories;
using SocialNetwork.DTOs.ViewModels;

public class CommentRepositories : ICommentRepositories
{
    private readonly SocialNetworkdDataContext _context;

    public CommentRepositories(SocialNetworkdDataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommentEntity>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task AddCommentAsync(CommentEntity commentEntity)
    {
        commentEntity.User = await _context.Users.FindAsync(commentEntity.UserID);
        //commentEntity.Post = await _context.Posts.FindAsync(commentEntity.PostID);
        var comment = await _context.Comments.AddAsync(commentEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(string commentId)
    {
        var comment = await GetCommentByIdAsync(commentId);
        if (comment != null)
        {
            comment.IsDelete = true;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }



    public async Task<CommentEntity> GetCommentByIdAsync(string commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }

    public async Task<IEnumerable<CommentViewModel>> GetCommentsByPostIdAsync(string postId)
    {
        return await _context.Comments
            .Include(x => x.User)
            .Where(x => x.PostID == postId && !x.IsDelete)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x=> new CommentViewModel
            {
                CommentID=x.CommentID,
                PostID=x.PostID,
                ParentCommentID=x.ParentCommentID,
                Content = x.Content,
                FirstName=x.User.FirstName,
                LastName=x.User.LastName,
                AvatarUrl=x.User.AvatarUrl,
                CreatedAt=x.User.CreatedAt, 
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<CommentEntity>> GetRepliesByCommentIdAsync(string parentCommentId)
    {
        return await _context.Comments
            .Where(x => x.ParentCommentID == parentCommentId && !x.IsDelete)
            .Include(x => x.User)
            .Include(x=>x.Replies)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task UpdateCommentAsync(CommentEntity comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetCommentCountByPostIdAsync(string postId)
    {
        var count=await _context.Comments.CountAsync(x=>x.PostID==postId&&!x.IsDelete);
        return count;
    }
}
