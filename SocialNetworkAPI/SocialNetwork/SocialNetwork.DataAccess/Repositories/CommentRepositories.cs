using SocialNetwork.DataAccess.Repositories;

public class CommentRepositories :ICommentRepositories
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
        //commentEntity.CommentID = Guid.NewGuid().ToString();
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

    public async Task<IEnumerable<CommentEntity>> GetCommentsByPostIdAsync(string postId)
    {
        return await _context.Comments
            .Where(x => x.PostID == postId && !x.IsDelete)
            .Include(x => x.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<CommentEntity>> GetRepliesByCommentIdAsync(string parentCommentId)
    {
        return await _context.Comments
            .Where(x => x.ParentCommentID == parentCommentId && !x.IsDelete)
            .Include(x => x.User)
            .ToListAsync();
    }

    public async Task UpdateCommentAsync(CommentEntity comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }
}
