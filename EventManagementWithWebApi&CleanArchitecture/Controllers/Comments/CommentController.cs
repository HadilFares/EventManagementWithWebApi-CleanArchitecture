using Application.Dtos.Comments;
using Application.Interfaces.CommentRepository;
using Application.Interfaces.EventRepository;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Comments
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        ICommentService _commentRepository;
        public CommentController(ICommentService commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> LikeComment(Guid id)
        {
            var comment = await _commentRepository.Get(id);
            if (comment == null)
            {
                return NotFound();
            }
            comment.Likes++;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();

            return NoContent();
        }
        [HttpPost("{id}/dislike")]
        public async Task<IActionResult> DislikeComment(Guid id)
        {
            var comment = await _commentRepository.Get(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.Likes > 0)
            {
                comment.Likes--;
            }

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();

            return NoContent();
        }


            [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(Guid id)
        {
            var comment = await _commentRepository.Get(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(string id ,Guid Eventid,[FromBody] CommentDTO commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = new Comment
            {
                UserId = id,
                EventId=Eventid,
                Text = commentDto.Text,
                Date = DateTime.Now,
                Likes = 0
               
            };

            _commentRepository.Create(comment);
            await _commentRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentDTO commentDto)
        {
            var existingComment = await _commentRepository.Get(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            existingComment.Text = commentDto.Text;

            _commentRepository.Update(existingComment);
            await _commentRepository.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var existingComment = await _commentRepository.Get(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            _commentRepository.Delete(id);
            await _commentRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
