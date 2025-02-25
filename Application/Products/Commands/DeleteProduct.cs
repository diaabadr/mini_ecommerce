using System;
using Application.DTOs;
using Domain.Common;
using MediatR;
using Persistence;

namespace Application.Products.Commands;

public class DeleteProduct
{
    public class Command : IRequest<ServiceResponse<string>>
    {
        public required string Id { get; set; }
    }

    public class Handler(IProductRepository productRepository) : IRequestHandler<Command, ServiceResponse<string>>
    {
        public async Task<ServiceResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var affectedRows = await productRepository.DeleteProductAsync(request.Id, cancellationToken);
            if (affectedRows == 0)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 404);
            }

            return ServiceResponse<string>.SuccessResponse("deleted successfully", 204);
        }
    }
}