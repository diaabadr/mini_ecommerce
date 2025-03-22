using Application.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.Queries
{
    public class GetReviews
    {
        public class Query : IRequest<ServiceResponse<List<ReviewDto>>>
        {
            public required string ProductId { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, ServiceResponse<List<ReviewDto>>>
        {
            async Task<ServiceResponse<List<ReviewDto>>> IRequestHandler<Query, ServiceResponse<List<ReviewDto>>>.Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await context.Reviews.Where(x => x.ProductId == request.ProductId).OrderByDescending(x => x.CreatedAt)
                    .ProjectTo<ReviewDto>(mapper.ConfigurationProvider).ToListAsync();

                return ServiceResponse<List<ReviewDto>>.SuccessResponse(result, 200);
            }
        }
    }
}
