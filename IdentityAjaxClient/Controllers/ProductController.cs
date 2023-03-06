using System.Diagnostics;
using System.Net.Http.Headers;
using BusinessObjects.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAjaxClient.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly HttpClient? _client = null;
        private string url = "https://localhost:7189/api/Products/";

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> productDtos = new List<ProductDto>();
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                productDtos = response.Content.ReadFromJsonAsync<List<ProductDto>>().Result;
            }
            return View(productDtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            ViewBag.Categories = await GetCategories();
            return View();
        }

        private async Task<List<CategoryDto>> GetCategories()
        {
            List<CategoryDto> categoryDtos = new List<CategoryDto>();
            HttpResponseMessage response = await _client.GetAsync(url + "Categories");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                categoryDtos = response.Content.ReadFromJsonAsync<List<CategoryDto>>().Result;
            }

            return categoryDtos;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ProductDto model)
        {
            ViewBag.Categories = await GetCategories();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpResponseMessage response = await _client.PostAsJsonAsync(url + "Create", model);
            var result = response.Content.ReadFromJsonAsync<bool>().Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK && result)
            {
                ViewData["msg"] = "Create Success!";
            }
            else
            {
                ViewData["msg"] = "Create Failed. Try again!";
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            var product = await GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            HttpResponseMessage response = await _client.DeleteAsync(url + $"Delete/{id}");
            var result = response.Content.ReadFromJsonAsync<bool>().Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK && result)
            {
                TempData["msg"] = "Delete Success!";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else

            {
                TempData["msg"] = "Delete Failed. Try again!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAsync(int? id)
        {
            var product = await GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await GetCategories();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync([FromForm] ProductDto model)
        {
            ViewBag.Categories = await GetCategories();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            HttpResponseMessage response = await _client.PutAsJsonAsync(url + "Update", model);
            var result = response.Content.ReadFromJsonAsync<bool>().Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK && result)
            {
                ViewData["msg"] = "Update Success!";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            else
            {
                ViewData["msg"] = "Update Failed. Try again!";
            }

            return View(model);
        }

        private async Task<ProductDto?> GetByIdAsync(int? id)
        {
            if (id == null) return null;

            HttpResponseMessage response = await _client.GetAsync(url + id);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content.ReadFromJsonAsync<ProductDto>().Result;
            }

            return null;
        }
    }
}
