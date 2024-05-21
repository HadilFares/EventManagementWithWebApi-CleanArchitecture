using Application.Interfaces.CommentRepository;
using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CommentService :ICommentService
    {

        private readonly IBaseRepository<Comment> _baseRepository;

        private readonly EventlyDbContext _context;

        public CommentService(EventlyDbContext context, IBaseRepository<Comment> baseRepository) 
        {
            _context = context;
            _baseRepository = baseRepository;
        }

        public  async Task<Comment> CreateComment(Comment comment)
        {
          await  _baseRepository.Create(comment);
            return comment;
        }

        public async Task<bool> DeleteComment(Guid id)
        {
            return await _baseRepository.Delete(id);

        }

        public async Task<List<Comment>> GetAllComments()
        {
            return await _baseRepository.GetAll();

        }

        public async Task<Comment> GetComment(Guid id)
        {
            return await _baseRepository.Get(id);
        }

        public async Task<Comment> UpdateComment(Comment comment)
        {
          await  _baseRepository.Update(comment);
            return comment;
        }
    }
}
