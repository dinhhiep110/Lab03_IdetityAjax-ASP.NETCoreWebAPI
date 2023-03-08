using System.Diagnostics;
using System.Net.Http.Headers;
using BusinessObjects;
using System.Text.Json;
using BusinessObjects.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAjaxClient.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        //private IProductRepository productsRepository = new ProductRepository();

        private string ProductApiUrl;

        private string CategoryApiUrl;

        public ProductController()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7189/api/Products";
            CategoryApiUrl = "https://localhost:7189/api/Category";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Category"] = await GetCategories();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto product)
        {
            int categoryId = product.CategoryId;

            using (var respone = await _httpClient.PostAsJsonAsync(ProductApiUrl, product))
            {
                string apiResponse = await respone.Content.ReadAsStringAsync();
            }
            return Redirect("/Product/Index");
        }

        public async Task<IActionResult> EditProduct([FromForm] ProductDto product)
        {
            using (var respone = await _httpClient.PutAsJsonAsync(ProductApiUrl + "/id?id=" + product.ProductId, product))
            {
                string apiResponse = await respone.Content.ReadAsStringAsync();
            }
            return Redirect("/Product/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            ViewData["Category"] = await GetCategories();
            List<ProductDto> products = await GetProducts();
            ProductDto product = products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            List<ProductDto> products = await GetProducts();
            ProductDto product = products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            List<ProductDto> products = await GetProducts();
            ProductDto product = products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            String url = ProductApiUrl + "/id?id=" + id;
            await _httpClient.DeleteAsync(url);
            return Redirect("/Product/Index");
        }

        private async Task<List<CategoryDto>> GetCategories()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(CategoryApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<CategoryDto>>(strData, options);
        }

        private async Task<List<ProductDto>> GetProducts()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<ProductDto>>(strData, options);
        }
    }
}
