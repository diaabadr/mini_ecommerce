using System;
using Application.DTOs;
using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Persistence;

namespace Application.Products.Commands;

public class UpdateProduct
{
    public class Command : IRequest<ServiceResponse<string>>
    {
        public required string Id { get; set; }

        public required UpdateProductDto Dto { get; set; }
    }

    public class Handler(IProductRepository productRepo, ICategoryRepository categoryRepo,
    IUserAccessor userAccessor) : IRequestHandler<Command, ServiceResponse<string>>
    {
        public async Task<ServiceResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var isExist = await categoryRepo.IsExistAsync(request.Dto.CategoryId, cancellationToken);
            if (!isExist)
            {
                return ServiceResponse<string>.
                    ErrorResponse(ErrorCodes.CategoryDoesNotExist, "Category does not exist", 400);
            }

            var user = await userAccessor.GetUserAsync();
            var product = new Product
            {
                Name = request.Dto.Name,
                CategoryId = request.Dto.CategoryId,
                Price = request.Dto.Price,
                StockQuanitty = request.Dto.StockQuanitty,
            };

            var affectedRows = await productRepo.UpdateProductAsync(request.Id, product, cancellationToken);
            if (affectedRows == 0)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 400);
            }

            return ServiceResponse<string>.SuccessResponse("updated successfully", 200);
        }
    }

}