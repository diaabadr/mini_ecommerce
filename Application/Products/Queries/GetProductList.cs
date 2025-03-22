using System;
using Application.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.Products.Queries
{
    public class GetProductList
    {
        public class Query : IRequest<ServiceResponse<List<ProductDto>>> { }

        public class Handler(AppDbContext dbContext, IMapper mapper) : IRequestHandler<Query, ServiceResponse<List<ProductDto>>>
        {

            public async Task<ServiceResponse<List<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await dbContext.products.AsNoTracking().ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

                return ServiceResponse<List<ProductDto>>.SuccessResponse(products, 200);
            }
        }
    }
}