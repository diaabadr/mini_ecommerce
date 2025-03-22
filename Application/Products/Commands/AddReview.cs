using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.Commands
{
    public class AddReview
    {
        public class Command : IRequest<ServiceResponse<ReviewDto>>
        {
            public required int Rate { get; set; }
            public string Comment { get; set; }

            public required string ProductId { get; set; }
        }

        public class Handler(AppDbContext context, IUserAccessor userAccessor, IMapper mapper) : IRequestHandler<Command, ServiceResponse<ReviewDto>>
        {

            public async Task<ServiceResponse<ReviewDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await context.products.Include(x => x.Reviews).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == request.ProductId);

                if (product == null)
                {
                    return ServiceResponse<ReviewDto>.ErrorResponse(ErrorCodes.ProductNotFound, "Product not found", 404);
                }

                var user = await userAccessor.GetUserAsync();

                var review = new Review
                {
                    UserId = user.Id,
                    ProductId = request.ProductId,
                    Comment = request.Comment,
                    Rate = request.Rate,
                    CreatedAt = DateTime.UtcNow
                };

                product.Reviews.Add(review);

                var result = await context.SaveChangesAsync(cancellationToken);

                return result > 0 ? ServiceResponse<ReviewDto>.SuccessResponse(mapper.Map<ReviewDto>(review), 201) :
                    ServiceResponse<ReviewDto>.ErrorResponse(ErrorCodes.InternalServerError, "internal server error", 500);
            }

        }
    }
}
