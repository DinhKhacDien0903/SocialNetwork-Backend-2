﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.IRepositories
{
    public interface ICommentRepositories : IBaseRepository<CommentEntity>
    {
        Task<IEnumerable<CommentEntity>> GetCommentsByPostIdAsync(Guid postId);
        Task<IEnumerable<CommentEntity>> GetRepliesByCommentIdAsync(Guid parentCommentId);
        Task<CommentEntity> AddCommentAsync(CommentEntity commentEntity);
        Task DeleteAsync(Guid commentId);
        Task UpdateAsync(CommentEntity comment);
        Task<CommentEntity> GetCommentByIdAsync(Guid commentId);
    }
}
