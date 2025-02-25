using Application.DTOs;
using Domain.Entities;

namespace Application.Service
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetAllProducts();
        Task<ServiceResponse<Product>> CreateProduct(CreateProductDto product);
        Task<ServiceResponse<string>> UpdateProduct(string id, Product product);
        Task<ServiceResponse<string>> DeleteProduct(string id);
        Task<ServiceResponse<Product>> GetById(string id);



    }
}
