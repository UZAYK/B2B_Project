using Microsoft.AspNetCore.Http;

namespace Entities.Concrete
{
    public class ProductImageAddDto
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; }
    }
}