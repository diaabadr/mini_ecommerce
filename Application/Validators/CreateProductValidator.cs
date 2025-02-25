using Application.DTOs;
using Domain.Common;
using FluentValidation;

namespace Application.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithErrorCode(ErrorCodes.ProductNameIsRequired).
                WithMessage("Name is required").MinimumLength(2).WithErrorCode(ErrorCodes.ProductNameShort).
                WithMessage("Product name should be more than 3 chars");

            RuleFor(p => p.Price).GreaterThan(0).WithErrorCode(ErrorCodes.PriceIsLow).
                WithMessage("product price should be greater than 0");

            RuleFor(p => p.StockQuanitty).GreaterThanOrEqualTo(0).WithErrorCode(ErrorCodes.NegativeQunatity).
              WithMessage("Stock quantity cannot be negative");
        }

    }
}
