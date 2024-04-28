using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CommentRepository
{
    public interface ICommentService
    {

        Task<Comment> GetComment(Guid id);
        Task<Comment> CreateComment(Comment comment);
        Task<Comment> UpdateComment(Comment comment);
        Task<bool> DeleteComment(Guid id);
        Task<List<Comment>> GetAllComments();
    }
}
