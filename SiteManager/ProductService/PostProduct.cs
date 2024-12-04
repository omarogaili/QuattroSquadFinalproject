using Microsoft.AspNetCore.Mvc;
using SiteManager;
using SiteManager.ProductService;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.Controllers;
using ProductValidation;

namespace ProductService
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostProductController : UmbracoApiController
    {
        private readonly IContentService _contentService;
        private readonly ProductValidator _productValidation;
        public PostProductController(IContentService contentService, ProductValidator productValidator)
        {
            _contentService = contentService;
            _productValidation = productValidator;

        }
        [HttpPost("products")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostProduct([FromBody] Product newProduct)
        {
            if (newProduct == null)
            {
                return BadRequest("Invalid product data.");
            }

            var rootContent = _contentService.GetRootContent().FirstOrDefault(x => x.ContentType.Alias == "productHandler");
            if (rootContent == null)
            {
                return NotFound("Root content (productHandler) not found.");
            }
            var productPage = CreateProductPage(rootContent.Id, newProduct);
            return CreatedAtAction(nameof(PostProduct), new { id = productPage.Id }, newProduct);
        }

        public IContent CreateProductPage(int parentId, Product newProduct)
        {
            var productPage = _contentService.Create("New Product", parentId, "productPage");
            productPage.SetValue("title", newProduct.Title);
            productPage.SetValue("category", newProduct.Category);
            productPage.SetValue("longDescription", newProduct.Description);
            productPage.SetValue("price", newProduct.Price);
            productPage.SetValue("description", newProduct.CardDescription);
            if (!string.IsNullOrEmpty(newProduct.ImageUrl))
            {
                productPage.SetValue("image", newProduct.ImageUrl);
            }
            
            _contentService.SaveAndPublish(productPage);
            return productPage;
        }
    }
}
