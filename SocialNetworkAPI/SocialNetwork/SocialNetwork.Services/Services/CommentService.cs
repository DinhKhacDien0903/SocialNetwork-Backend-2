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

        public CommentService(ICommentRepositories commentRepositories, IMapper mapper)
        {
            _commentRepositories = commentRepositories;
            _mapper = mapper;
        }

        public async Task<CommentRequest> AddCommentAsycn(CommentRequest commentRequest)
        {
            // Kiểm tra xem bình luận gốc có tồn tại không
            //if (commentRequest.ParentCommentID.HasValue)
            //{
            //    var parentCommentExists = await _context.Comments.AnyAsync(c => c.CommentID == commentRequest.ParentCommentID.Value);
            //    if (!parentCommentExists)
            //    {
            //        throw new Exception("Parent comment not found."); // Hoặc xử lý lỗi khác phù hợp
            //    }
            //}

            // Sử dụng AutoMapper để ánh xạ từ CommentRequest sang CommentEntity
            var commentEntity = _mapper.Map<CommentEntity>(commentRequest);
            commentEntity.CommentID = Guid.NewGuid(); // Tạo mới ID cho bình luận

            // Thêm vào DbContext và lưu
            var addedComment = await _commentRepositories.AddCommentAsync(commentEntity);

            // Trả về CommentRequest đã được ánh xạ từ CommentEntity
            return _mapper.Map<CommentRequest>(addedComment);
        }









        public async Task DeleteCommentAsycn(Guid commentId)
        {
            await _commentRepositories.DeleteAsync(commentId);
        }

        public async Task<IEnumerable<CommentViewModel>> GetAllCommentAsync()
        {
            var comment = await _commentRepositories.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentViewModel>>(comment);
        }

        public async Task<CommentViewModel> GetCommentByIdAsync(Guid commetId)
        {
            var comment = await _commentRepositories.GetCommentByIdAsync(commetId);
            return _mapper.Map<CommentViewModel>(comment);

        }

        public async Task<IEnumerable<CommentViewModel>> GetCommentByPostIdAsycn(Guid postId)
        {
            var comment = await _commentRepositories.GetCommentsByPostIdAsync(postId);
            return _mapper.Map<IEnumerable<CommentViewModel>>(comment);
        }

        public async Task<IEnumerable<CommentViewModel>> GetRepliesByCommentIdAsycn(Guid parentCommentId)
        {
            var comment = await _commentRepositories.GetRepliesByCommentIdAsync(parentCommentId);
            return _mapper.Map<IEnumerable<CommentViewModel>>(comment);

        }

        public async Task UpdateCommentAsycm(CommentViewModel commentViewModel)
        {
            var comment = _mapper.Map<CommentEntity>(commentViewModel);
            await _commentRepositories.UpdateAsync(comment);
        }
    }
}
