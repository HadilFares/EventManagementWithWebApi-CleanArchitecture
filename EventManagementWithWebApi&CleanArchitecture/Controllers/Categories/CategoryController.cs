using Application.Dtos.Category;
using Application.Interfaces.CategoryRepository;
using Application.Interfaces.IBaseRepository;
using Domain.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EventManagementWithWebApi_CleanArchitecture.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryService _categoryRepository;
        public CategoryController(ICategoryService categoryRepository ) {
            _categoryRepository=categoryRepository;
        }
        [HttpGet]
        [Route("GetAllCategories")]
      //  [Authorize]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return categories;
        }

        [HttpGet]
        [Route("GetAllNamesCategories")]
        //  [Authorize]
        public async Task<ActionResult<List<String>>> GetAllNameCategories()
        {
            var categories = await _categoryRepository.GetAllNamesCategories();
            return categories;
        }



        [HttpPost]
        //[Authorize()]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = await _categoryRepository.GetCategoryByName(categoryDto.Name);
            if (existingCategory != Guid.Empty)
            {
                // If a category with the same name exists, return a conflict response
                return Conflict("A category with the same name already exists.");
            }



            var category = new Category
            {
                Name = categoryDto.Name,
                OrganizerId = categoryDto.UserId
            };

            _categoryRepository.CreateCategory(category);

            return CreatedAtAction(nameof(GetCatagoryById), new { id = category.Id }, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var deleted = await _categoryRepository.DeleteCategory(id);

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
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id ,[FromBody] CategoryDTO categoryDto)
        {
          
            var existingCategory = await _categoryRepository.GetCategory(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            var existingNameCategory = await _categoryRepository.GetCategoryByName(categoryDto.Name);
            if (existingNameCategory != Guid.Empty)
            {
                // If a category with the same name exists, return a conflict response
                return Conflict("A category with the same name already exists.");
            }



            existingCategory.Name = categoryDto.Name;
           

            _categoryRepository.UpdateCategory(existingCategory);
       

            return NoContent();
        
        }

        [HttpGet("GetCategoriesByUserId/{userId}")]
        public async Task<IActionResult> GetCategoriesByUserId(string userId)
        {
            var categories = await _categoryRepository.GetCategoriesByUserId(userId);
            if (categories == null || !categories.Any())
            {
                return NotFound();
            }

            return Ok(categories);
        }
        [HttpGet("GetCategoryByName/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var category = await _categoryRepository.GetCategoryByName(name);
            if (category==Guid.Empty)
            {
                return NotFound("Category doesn't exist");
            }

            return Ok(category);
        }

    }
}
