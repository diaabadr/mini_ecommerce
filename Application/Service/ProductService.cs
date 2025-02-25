using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using Persistence;

namespace Application.Service
{
    public class ProductService : IProductService
    {

        public readonly IValidator<CreateProductDto> _createValidator;

        private readonly IProductRepository _productRepo;

        private readonly ICategoryRepository _categoryRepo
            ;
        public ProductService(
            IProductRepository productRepo,
            ICategoryRepository categoryRepo,
            IValidator<CreateProductDto> createValidator
            )
        {
            this._productRepo = productRepo;
            this._categoryRepo = categoryRepo;
            this._createValidator = createValidator;
        }

        public async Task<ServiceResponse<List<Product>>> GetAllProducts()
        {
            var products = await _productRepo.GetAllProducts();
            return ServiceResponse<List<Product>>.SuccessResponse(products, 200);
        }

        public async Task<ServiceResponse<Product>> CreateProduct(CreateProductDto dto)
        {

            var isExist = await this._categoryRepo.IsExistAsync(dto.CategoryId);
            if (!isExist)
            {
                return ServiceResponse<Product>.
                    ErrorResponse(ErrorCodes.CategoryDoesNotExist, "category not found", 400);
            }

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Price = dto.Price,
                StockQuanitty = dto.StockQuanitty
            };

            var Product = await this._productRepo.AddProductAsync(product);
            if (product == null)
            {
                return ServiceResponse<Product>.
                    ErrorResponse(ErrorCodes.InternalServerError, "internal server error", 500);
            }

            return ServiceResponse<Product>.SuccessResponse(product, 201);

        }

        public async Task<ServiceResponse<string>> UpdateProduct(string id, Product product)
        {
            var isExist = await this._categoryRepo.IsExistAsync(product.CategoryId);
            if (!isExist)
            {
                return ServiceResponse<string>.
                    ErrorResponse(ErrorCodes.CategoryDoesNotExist, "Category does not exist", 400);
            }

            var affectedRows = await this._productRepo.UpdateProductAsync(id, product);
            if (affectedRows == 0)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 400);
            }

            return ServiceResponse<string>.SuccessResponse("updated successfully", 204);
        }

        public async Task<ServiceResponse<string>> DeleteProduct(string id)
        {
            var affectedRows = await this._productRepo.DeleteProductAsync(id);
            if (affectedRows == 0)
            {
                return ServiceResponse<string>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 404);
            }

            return ServiceResponse<string>.SuccessResponse("deleted successfully", 204);
        }

        public async Task<ServiceResponse<Product>> GetById(string id)
        {
            var product = await this._productRepo.GetByIdAsync(id);

            if (product == null)
            {
                return ServiceResponse<Product>.ErrorResponse(ErrorCodes.ProductNotFound, "product not found", 404);
            }

            return ServiceResponse<Product>.SuccessResponse(product, 200);
        }
    }
}


