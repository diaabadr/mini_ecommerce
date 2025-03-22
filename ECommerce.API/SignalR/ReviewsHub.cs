using Application.Products.Commands;
using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.SignalR
{
    public class ReviewsHub(IMediator mediator, ILogger<ReviewsHub> logger) : Hub
    {

        public async Task SendReview(AddReview.Command command)
        {
            Console.WriteLine("Diaa");
            var review = await mediator.Send(command);

            await Clients.Group(command.ProductId).SendAsync("ReceiveReview", review.Data);
        }
        public override async Task OnConnectedAsync()
        {
            logger.LogInformation("Connected to hub");

            var httpContext = Context.GetHttpContext();

            var productId = httpContext?.Request.Query["productId"];

            if (string.IsNullOrEmpty(productId)) throw new HubException("No Product with this id");

            await Groups.AddToGroupAsync(Context.ConnectionId, productId!);

            var result = await mediator.Send(new GetReviews.Query { ProductId = productId! });

            await Clients.Caller.SendAsync("LoadReviews", result.Data);

        }
    }
}
