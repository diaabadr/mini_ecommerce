using System;
using Application.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class GetProductDetails
{
    public class Query : IRequest<ServiceResponse<ProductDto>>
    {
        public required string Id { get; set; }
    }

    public class Handler(IMapper mapper, AppDbContext dbContext) : IRequestHandler<Query, ServiceResponse<ProductDto>>
    {

        public async Task<ServiceResponse<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var product = await dbContext.products.AsNoTracking().Include(p => p.Creator).ProjectTo<ProductDto>(mapper.ConfigurationProvider).
           FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return ServiceResponse<ProductDto>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 404);
            }

            return ServiceResponse<ProductDto>.SuccessResponse(product, 200);
        }
    }
}