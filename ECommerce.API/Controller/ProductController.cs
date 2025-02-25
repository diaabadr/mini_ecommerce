using Application.DTOs;
using Application.Service;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace ECommerce.API.Controllers
{
    public class ProductController : BaseAPIController
    {

        private readonly IProductService _productService;
        public ProductController(IProductService productService, ICategoryRepository categoryRepo)
        {
            this._productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var res = await _productService.GetAllProducts();
            return HandleResponse(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var res = await this._productService.CreateProduct(dto);
            return HandleResponse(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductDto product)
        {
            var res = await this._productService.UpdateProduct(id, product);

            return HandleResponse(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var res = await this._productService.DeleteProduct(id);
            return HandleResponse(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await this._productService.GetById(id);


            return HandleResponse(res);
        }
    }
}
