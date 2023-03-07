using AutoMapper;
using BusinessObjects;
using BusinessObjects.Dtos;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository productsRepository = new ProductRepository();

        private readonly IMapper _mapper;

        public ProductsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        //Get: api/Products
        [HttpGet]
        public List<ProductDto> GetProducts() => _mapper.Map<List<ProductDto>>(productsRepository.GetProducts());


        //Post :ProductsController/Products
        [HttpPost]
        public IActionResult PostProduct([FromBody]ProductDto product)
        {
            Product p = _mapper.Map<Product>(product);
            productsRepository.SaveProduct(p);
            return NoContent();
        }

        //Get: ProductsController/Delete/5
        [HttpDelete("id")]
        public IActionResult DeleteProduct(int id)
        {
            var product = productsRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            productsRepository.DeleteProduct(product);
            return NoContent();
        }


        //Get: ProductsController/Delete/5
        [HttpPut("id")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDto product)
        {
            var p = productsRepository.GetProductById(id);
            if(p == null)
            {
                return NotFound();
            }
            p = _mapper.Map<Product>(product);
            p.ProductId = id;
            productsRepository.UpdateProduct(p);
            return NoContent();
        }

    }
}
