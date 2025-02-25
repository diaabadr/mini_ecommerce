using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateProductValidator : CreateProductValidator
    {
        public UpdateProductValidator()
        {
            // We can set whatever extra rule we need on update here, and 
            // If there is no extra rules we can treat the IValidator dependency as CreateProductValidator in program.cs 
            // and no need for that class to exist in that case
        }
    }
}