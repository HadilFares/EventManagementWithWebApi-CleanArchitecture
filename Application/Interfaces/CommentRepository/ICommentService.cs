using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CommentRepository
{
    public interface ICommentService:IBaseRepository<Comment>
    {
    }
}
