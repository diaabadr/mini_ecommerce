using Domain;
using Domain.Common;
using Domain.Entities;
using ECommerce.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Persistence;


namespace Test
{
    public class ProductControllerTest
    {

        private Mock<IProductRepository> _mockProductRepo;
        private Mock<ICategoryRepository> _mockCategoryRepo;
        private ProductController _productController;


        [SetUp]
        public void Setup()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _productController = new ProductController(_mockProductRepo.Object, _mockCategoryRepo.Object);
        }

        [Test]
        public async Task GetAllProducts_ReturnsOk_WithProductsList()
        {

            var mockProducts = new List<Product> {
            new Product{Id = "1", Name = "Citizen Watch", Price= 200, StockQuanitty=3, CategoryId = "1"},
            new Product{Id = "2", Name = "Rolex Watch", Price = 3000, StockQuanitty = 2, CategoryId = "2"},
            };

            _mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(mockProducts);


            var result = await _productController.GetAllProducts();

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.AssignableTo<List<Product>>());

            var returnedProducts = okResult.Value as List<Product>;
            Assert.That(returnedProducts, Is.Not.Null);
            Assert.That(returnedProducts.Count, Is.EqualTo(mockProducts.Count));
        }

        [Test]
        public async Task GetAllProdcuts_ReturnsOk_WithEmptyList_WhenNoProducts()
        {
            _mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(new List<Product>());

            var result = await _productController.GetAllProducts();

            var okResult = result.Result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            var returnedProducts = okResult.Value as List<Product>;

            Assert.That(returnedProducts, Is.Not.Null);
            Assert.That(0, Is.EqualTo(0));
        }


        [Test]
        public async Task CreateProduct_ReturnsBadRequest_WhenProductIsNull()
        {
            var result = await _productController.CreateProduct(null);

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var badRequest = result.Result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.InstanceOf<CustomErrorResponse>());

            var errorResponse = badRequest.Value as CustomErrorResponse;

            Assert.That(errorResponse, Is.Not.Null);

            Assert.That(errorResponse.ErrorCode, Is.EqualTo(ErrorCodes.ProductDataIsRequired));
        }

        [Test]
        public async Task CreateProduct_ReturnsBadRequest_WhenCategoryNotFound()
        {
            var product = new Product { Name = "Rolex Watch", CategoryId = "2", Price = 30, StockQuanitty = 3 };

            _mockCategoryRepo.Setup(repo => repo.IsExistAsync(product.CategoryId)).ReturnsAsync(false);
            var result = await _productController.CreateProduct(product);

            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());

            var badRequest = result.Result as BadRequestObjectResult;

            Assert.That(badRequest.Value, Is.InstanceOf<CustomErrorResponse>());

            var errorResponse = badRequest.Value as CustomErrorResponse;

            Assert.That(errorResponse.ErrorCode, Is.EqualTo(ErrorCodes.CategoryDoesNotExist));
        }

        [Test]
        public async Task CreateProduct_ReturnsOk()
        {
            var product = new Product { Name = "Rolex Watch", CategoryId = "2", Price = 30, StockQuanitty = 3 };
            _mockCategoryRepo.Setup(repo => repo.IsExistAsync(product.CategoryId)).ReturnsAsync(true);

            _mockProductRepo.Setup(repo => repo.AddProductAsync(product)).ReturnsAsync(product);

            var result = await _productController.CreateProduct(product);

            Console.WriteLine(result);

            Assert.That(result.Value, Is.TypeOf<Product>());

            var createdResult = result.Value as Product;

            Assert.That(createdResult, Is.SameAs(product));

        }
    }

}