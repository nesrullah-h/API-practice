using ApiPractice.Data.DAL;
using ApiPractice.Data.Entities;
using ApiPractice.Dto;
using ApiPractice.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public ProductController(IWebHostEnvironment env, AppDbContext context)
        {
            _env=env;
            _context=context;
        }

       
        [HttpGet]
        public IActionResult Get()
        {
            //List<Product> products = _context.Products.Include(p=>p.Category).Where(c => c.IsDeleted==false).ToList();

            List<ProductReturnDto> productsList = _context.Products.Select(p => new ProductReturnDto
            {
                Name=p.Name,
                Price=p.Price,
                Desc=p.Desc,
                CategoryName=p.Category.Name,
                ImageUrl=$"https://localhost:44358/img/{p.ImageUrl}"
        }).ToList();

            return StatusCode(200, productsList);
            //return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _context.Products.Include(p => p.Category).Where(c => c.IsDeleted==false).FirstOrDefault(p=>p.Id==id);
            if (product==null)
            {
                return NotFound();
            }
            product.ImageUrl=$"https://localhost:44358/img/{product.ImageUrl}";

            return StatusCode(200, product);
            //return Ok(products);
        }


        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto productCreateDto)
        {
            Product newProduct = new Product();
            if (!productCreateDto.Photo.IsImage())
            {
                return BadRequest("shekil formati secin");
            }

            if (productCreateDto.Photo.CheckSize(200000))
            {
                return BadRequest("olcu chox boyukdur");
            }

            newProduct.Name=productCreateDto.Name;
            newProduct.Desc=productCreateDto.Desc;
            newProduct.CategoryId=productCreateDto.CategoryId;
            newProduct.Price=productCreateDto.Price;
            newProduct.ImageUrl=await productCreateDto.Photo.SaveImage(_env,"img");
            _context.Add(newProduct);
            _context.SaveChanges();
            return StatusCode(201, newProduct);
        }
    }
}
