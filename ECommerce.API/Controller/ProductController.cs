using Application.DTOs;
using Application.Products.Commands;
using Application.Products.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace ECommerce.API.Controllers
{
    public class ProductController : BaseAPIController
    {

        public ProductController() { }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var res = await Mediator.Send(new GetProductList.Query());
            return HandleResponse(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            var res = await Mediator.Send(new CreateProduct.Command { Dto = dto });
            return HandleResponse(res);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductDto dto)
        {
            var res = await Mediator.Send(new UpdateProduct.Command { Id = id, Dto = dto });
            return HandleResponse(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var res = await Mediator.Send(new DeleteProduct.Command { Id = id });
            return HandleResponse(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var res = await this.Mediator.Send(new GetProductDetails.Query { Id = id });
            return HandleResponse(res);
        }
    }
}
