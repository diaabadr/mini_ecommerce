using System;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Products.Commands;

public class CreateProduct
{
    public class Command : IRequest<ServiceResponse<ProductDto>>
    {
        public required CreateProductDto Dto { get; set; }
    }

    public class Handler(IProductRepository productRepo, ICategoryRepository categoryRepo,
    IUserAccessor userAccessor, IMapper mapper)
     : IRequestHandler<Command, ServiceResponse<ProductDto>>
    {

        public async Task<ServiceResponse<ProductDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userAccessor.GetUserAsync();
            var dto = request.Dto;

            var isExist = await categoryRepo.IsExistAsync(dto.CategoryId, cancellationToken);
            if (!isExist)
            {
                return ServiceResponse<ProductDto>.
                    ErrorResponse(ErrorCodes.CategoryDoesNotExist, "category not found", 400);
            }

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                StockQuanitty = dto.StockQuanitty,
                CreatorId = user.Id
            };
            product = await productRepo.AddProductAsync(product, user.Id, cancellationToken);
            if (product == null)
            {
                return ServiceResponse<ProductDto>.
                    ErrorResponse(ErrorCodes.InternalServerError, "internal server error", 500);
            }

            return ServiceResponse<ProductDto>.SuccessResponse(mapper.Map<ProductDto>(product), 201);
        }
    }
}