using System;
using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Products.Commands;

public class CreateProduct
{
    public class Command : IRequest<ServiceResponse<Product>>
    {
        public required CreateProductDto Dto { get; set; }
    }

    public class Handler(IProductRepository productRepo, ICategoryRepository categoryRepo) : IRequestHandler<Command, ServiceResponse<Product>>
    {

        public async Task<ServiceResponse<Product>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var isExist = await categoryRepo.IsExistAsync(dto.CategoryId, cancellationToken);
            if (!isExist)
            {
                return ServiceResponse<Product>.
                    ErrorResponse(ErrorCodes.CategoryDoesNotExist, "category not found", 400);
            }

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                StockQuanitty = dto.StockQuanitty
            };

            var Product = await productRepo.AddProductAsync(product, cancellationToken);
            if (product == null)
            {
                return ServiceResponse<Product>.
                    ErrorResponse(ErrorCodes.InternalServerError, "internal server error", 500);
            }

            return ServiceResponse<Product>.SuccessResponse(product, 201);
        }
    }
}