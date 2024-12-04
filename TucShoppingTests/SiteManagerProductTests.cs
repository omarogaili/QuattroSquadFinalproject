using System.Net;
using System.Net.Http.Json;
using SiteManager.ProductService;
using Xunit;

namespace TucShoppingTests
{
    public class CreateProductTests
    {
        private readonly HttpClient _httpClient;

        public CreateProductTests()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:44317/")
            };
        } 
        [Fact]
        public async void CreateProduct_ReturnsCreated_WhenValidProductIsPosted()
        {
            var newProduct = new Product
            {
                Title = "Test Product",
                Category = "Test Category",
                Description = "A test product",
                Price = 100,
                CardDescription = "Short description",
                ImageUrl = "http://example.com/image.jpg"
            };
            var response = await _httpClient.PostAsJsonAsync("api/PostProduct/products", newProduct);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
            Assert.NotNull(createdProduct);
            Assert.Equal(newProduct.Title, createdProduct.Title);
        }
        [Fact]
        public async void CreateProduct_ReturnsBadRequest_WhenInvalidProductPosted()
        {
            var invalidProduct = new Product
            {
                Title = "Test Product",
                Category = "Test Category",
                Description = "A test product",
                Price = -10,
                CardDescription = "Short description",
                ImageUrl = "http://example.com/image.jpg"
            };
            var response = await _httpClient.PostAsJsonAsync("api/PostProduct/products", invalidProduct);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
