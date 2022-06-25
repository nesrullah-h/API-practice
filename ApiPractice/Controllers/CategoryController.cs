using ApiPractice.Data.DAL;
using ApiPractice.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ApiPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context=context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Category> categories = _context.Categories.Where(c => c.IsDeleted==false).ToList();

            return StatusCode(200, categories);
            //return Ok(products);
        }



        [HttpGet("isdelete")]
        public IActionResult GetDeleted()
        {
            List<Category> categories = _context.Categories.Where(c => c.IsDeleted).ToList();

            return StatusCode(200, categories);
            //return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Category category = _context.Categories.Where(c => c.IsDeleted==false).FirstOrDefault(c=>c.Id==id);

            if (category==null) return NotFound();

            return StatusCode(200, category);
            //return Ok(products);
        }

        [HttpPost("")]
        public IActionResult Create(Category category)
        {
            bool isExistName = _context.Categories.Any(c=>c.Name==category.Name);

            if (isExistName)
            {
                return BadRequest("Already exist");
            }
            Category newCategory = new Category();
            newCategory.Name=category.Name;
            newCategory.Desc=category.Desc;
            _context.Add(newCategory);
            _context.SaveChanges();
            return StatusCode(201, newCategory);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int? id,Category category)
        {
            if (id==null) return NotFound();

            Category dbCategory=_context.Categories.Where(c=>c.IsDeleted==false).FirstOrDefault(c=>c.Id==id);

            if (dbCategory==null) return NotFound();

            Category dbCategoryWithName = _context.Categories.Where(c => c.IsDeleted==false).FirstOrDefault(c => c.Name==category.Name);

            if (dbCategory!=null)
            {
                if (dbCategory!=dbCategoryWithName)
                {
                    return BadRequest("Already exist");
                }
            }

            dbCategory.Name=category.Name;
            dbCategory.Desc=category.Desc;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Where(c => c.IsDeleted==false).FirstOrDefault(c => c.Id==id);

            if (category==null) return NotFound();
            category.IsDeleted=true;
            _context.SaveChanges();

            return StatusCode(200, category);
            //return Ok(products);
        }
    }
}
