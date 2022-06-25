using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPractice.Dto
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
