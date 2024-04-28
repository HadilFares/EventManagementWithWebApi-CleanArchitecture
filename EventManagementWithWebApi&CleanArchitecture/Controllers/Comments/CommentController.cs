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
            var comment = await _commentRepository.GetComment(id);
            if (comment == null)
            {
                return NotFound();
            }
            comment.Likes++;

            _commentRepository.UpdateComment(comment);


            return NoContent();
        }
        [HttpPost("{id}/dislike")]
        public async Task<IActionResult> DislikeComment(Guid id)
        {
            var comment = await _commentRepository.GetComment(id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.Likes > 0)
            {
                comment.Likes--;
            }

            _commentRepository.UpdateComment(comment);


            return NoContent();
        }


            [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetCommentById(Guid id)
        {
            var comment = await _commentRepository.GetComment(id);
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

            _commentRepository.CreateComment(comment);


            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] CommentDTO commentDto)
        {
            var existingComment = await _commentRepository.GetComment(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            existingComment.Text = commentDto.Text;

            _commentRepository.UpdateComment(existingComment);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var existingComment = await _commentRepository.GetComment(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            _commentRepository.DeleteComment(id);

            return NoContent();
        }
    }
}
