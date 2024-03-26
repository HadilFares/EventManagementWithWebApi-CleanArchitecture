using Application.Interfaces.CommentRepository;
using Domain.Entities;
using Infra.Data.BaseRepository;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Services
{
    public class CommentService:BaseRepository<Comment>,ICommentService
    {


        private readonly EventlyDbContext _context;

        public CommentService(EventlyDbContext context) : base(context)
        {
            _context = context;

        }
    }
}
