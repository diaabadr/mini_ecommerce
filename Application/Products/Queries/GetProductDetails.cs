using System;
using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Persistence;

public class GetProductDetails
{
    public class Query : IRequest<ServiceResponse<Product>>
    {
        public required string Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, ServiceResponse<Product>>
    {
        private readonly IProductRepository _productRepo;

        public Handler(IProductRepository productRepository)
        {
            _productRepo = productRepository;
        }
        public async Task<ServiceResponse<Product>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await this._productRepo.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
            {
                return ServiceResponse<Product>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 404);
            }

            return ServiceResponse<Product>.SuccessResponse(product, 200);
        }
    }
}