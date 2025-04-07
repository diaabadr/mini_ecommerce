using Application.DTOs;
using Application.Interfaces;
using Application.Products.Queries;
using Application.Validators;
using Domain;
using ECommerce.API.Middlewares;
using ECommerce.API.SignalR;
using FluentValidation;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console().CreateLogger();

builder.Host.UseSerilog();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUserAccessor, UserAccessor>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDto>, UpdateProductValidator>();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetProductList.Handler>());
builder.Services.AddIdentityApiEndpoints<User>(opts =>
{
    opts.User.RequireUniqueEmail = true;

}).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("IsProductCreator", policy =>
    {
        policy.Requirements.Add(new IsCreatorRequirment());
    });
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
builder.Services.AddSingleton<PrometheusDbInterceptor>();


builder.Services.AddTransient<IAuthorizationHandler, IsCreatorRequirmentHandler>();
var app = builder.Build();


app.UseRouting();

app.UseHttpMetrics(options =>
{
    options.RequestDuration.Enabled = true;
    options.AddCustomLabel("route", context =>
    {
        var endpoint = context.GetEndpoint();
        var routePattern = (endpoint as Microsoft.AspNetCore.Routing.RouteEndpoint)?.RoutePattern?.RawText;
        return routePattern ?? "unknown";
    });
});


app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api").MapIdentityApi<User>();

app.MapHub<ReviewsHub>("/reviewshub");
app.MapHub<ChatHub>("/chathub");

app.UseSwagger();
app.UseSwaggerUI();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>();
try
{
    context.Database.MigrateAsync().GetAwaiter().GetResult();
}
catch (Exception e)
{
    Log.Error("Migration failed: ", e);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();

app.MapMetrics();

app.MapControllers();

app.UseHttpsRedirection();


app.Run();
