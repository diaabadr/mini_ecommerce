using System;
using Application.DTOs;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Products.Queries
{
    public class GetProductList
    {
        public class Query : IRequest<ServiceResponse<List<Product>>> { }

        public class Handler(IProductRepository productRepo) : IRequestHandler<Query, ServiceResponse<List<Product>>>
        {

            public async Task<ServiceResponse<List<Product>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await productRepo.GetAllProducts(cancellationToken);
                return ServiceResponse<List<Product>>.SuccessResponse(products, 200);
            }
        }
    }
}