using Application.Dtos.Category;
using Application.Interfaces.IBaseRepository;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        IBaseRepository<Category> _categoryRepository;
        public CategoryController(IBaseRepository<Category> categoryRepository ) {
            _categoryRepository=categoryRepository;
        }
        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAll();
            return categories;
        }

     
        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                UserId = categoryDto.UserId
            };

            _categoryRepository.Create(category);
            await _categoryRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCatagoryById), new { id = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var deleted = await _categoryRepository.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent(); 
        }
        [HttpGet]
        [Route("GetCatagoryById/{id}")]
        public async Task<IActionResult> GetCatagoryById(Guid id)
        {
            var course = await _categoryRepository.Get(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDTO categoryDto)
        {
          
            var existingCategory = await _categoryRepository.Get(categoryDto.UserId);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = categoryDto.Name;
           

            _categoryRepository.Update(existingCategory);
            await _categoryRepository.SaveChangesAsync();

            return NoContent();
        
        }
    }
}
