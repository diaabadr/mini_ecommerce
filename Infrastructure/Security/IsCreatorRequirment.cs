using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security;

public class IsCreatorRequirment : IAuthorizationRequirement
{

}

public class IsCreatorRequirmentHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
: AuthorizationHandler<IsCreatorRequirment>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCreatorRequirment requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return;

        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext?.GetRouteValue("id") is not string productId) return;

        var creator = await dbContext.products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId && p.CreatorId == userId);

        if (creator == null) return;

        context.Succeed(requirement);
    }
}