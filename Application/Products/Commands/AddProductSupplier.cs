using System;
using Application.DTOs;
using AutoMapper;
using Domain;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Products.Commands;

public class AddProductSupplier
{
    public class Command : IRequest<ServiceResponse<string>>
    {
        public required string ProductId { get; set; }
        public required string SupplierId { get; set; }
    }

    public class Handler(AppDbContext dbContext, IMapper mapper) : IRequestHandler<Command, ServiceResponse<string>>
    {
        public async Task<ServiceResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var productTask = dbContext.products.AnyAsync(p => p.Id == request.ProductId);
            var supplierTask = dbContext.Users.AnyAsync(p => p.Id == request.SupplierId);

            await Task.WhenAll(productTask, supplierTask);

            bool productExists = await productTask;
            if (!productExists)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 404);
            }
            bool supplierExists = await supplierTask;
            if (!supplierExists)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.ProductNotFound, "supplier not found", 404);
            }

            var isExist = await dbContext.ProductSuppliers.AnyAsync(ps => ps.ProductId == request.ProductId && ps.UserId == request.SupplierId);
            if (isExist)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.ProductSupplierExist,
                 "Supplier is already assigned to that product", 400);
            }
            dbContext.ProductSuppliers.Add(mapper.Map<ProductSupplier>(request));
            var res = await dbContext.SaveChangesAsync();

            if (res == 0)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.InternalServerError, "internal server error", 500);
            }

            return ServiceResponse<string>.SuccessResponse("Added Successfully", 201);
        }
    }
}