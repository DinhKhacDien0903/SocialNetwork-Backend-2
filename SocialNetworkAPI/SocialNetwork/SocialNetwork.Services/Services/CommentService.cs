using SocialNetwork.DTOs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Services
{                               
    public class CommentService : ICommentService
    {
        private readonly ICommentRepositories _commentRepositories;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepositories  commentRepositories, IMapper mapper)
        {
            _commentRepositories = commentRepositories;
            _mapper = mapper;
        }
        public async Task<CommentViewModel> AddCommentAsycn(CommentViewModel commentViewModel)
        {
            var comments =  _mapper.Map<CommentEntity>(commentViewModel);
            var result= await _commentRepositories.AddCommentAsync(comments);
            return _mapper.Map<CommentViewModel>(result);   

        }
       

        public async Task DeleteCommentAsycn(Guid commentId)
        {
           var comment= await _commentRepositories.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new Exception("comment not found");
            }
            comment.IsDelete = true;
            await _commentRepositories.UpdateAsycn(comment);
        }
            
        public async Task<IEnumerable<CommentViewModel>> GetCommentByPostIdAsycn(Guid postId)
        {
            var commentById=await _commentRepositories.GetCommentsByPostIdAsync(postId);

            var commentViewModel= _mapper.Map<IEnumerable<CommentViewModel>>(commentById);
            //foreach (var comment in commentById)
            //{
            //    var replies= await _commentRepositories.GetRepliesByCommentIdAsync(comment.CommentID);
            //    comment.Replies= _mapper.Map<List<CommentViewModel>>(replies);
            //}
            return commentViewModel;
        }

        public async Task<IEnumerable<CommentViewModel>> GetRepliesByCommentIdAsycn(Guid parentCommentId)
        {
            var repliesByComment=await _commentRepositories.GetRepliesByCommentIdAsync(parentCommentId);
            return _mapper.Map<IEnumerable<CommentViewModel>>(parentCommentId);
        }

        public async Task<CommentViewModel> UpdateCommentAsycm(Guid commentId, CommentViewModel comment)
        {
            var commentEntity = await _commentRepositories.GetCommentByIdAsync(comment.CommentID);
            if (commentEntity == null)
            {
                throw new Exception("Comment not found");
            }

            commentEntity.Content = comment.Content;
            commentEntity.UpdatedAt = DateTime.UtcNow;

            var updatedComment = await _commentRepositories.UpdateAsycn(commentEntity);
            return _mapper.Map<CommentViewModel>(updatedComment);
        }
    }
}
