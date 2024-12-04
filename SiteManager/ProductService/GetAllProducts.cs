using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using System.Linq;
using SiteManager;
namespace SiteManager.ProductService;
[Route("api/[controller]")]
[ApiController]
public class GetAllProducts : UmbracoController
{
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    public GetAllProducts(IUmbracoContextAccessor umbracoContextAccessor)
    {
        _umbracoContextAccessor = umbracoContextAccessor;
    }
    [HttpGet("products")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetProductsWithDetails()
    {
        var productHandlers = GetProductsWithDetalis();
        if (!productHandlers.Any())
        {
            return NotFound();
        }
        return Ok(productHandlers);
    }
    public List<Product> GetProductsWithDetalis()
    {
        var umbracoContext = _umbracoContextAccessor.GetRequiredUmbracoContext();
        var request = HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        var productHandlers = umbracoContext.Content!.GetAtRoot()
            .Where(x => x.ContentType.Alias == "productHandler")
            .SelectMany(x => x.Children)
            .Where(x => x.ContentType.Alias == "productPage")
            .Select(x => new Product
            {
                Id = x.Id,
                Title = x.Value<string>("Title"),
                Category = x.Value<string>("category"),
                Description = x.Value<string>("longDescription"),
                Price = x.Value<int>("price"),
                CardDescription = x.Value<string>("description"),
                ImageUrl = CreateProductsImageUrl(x, baseUrl)
            })
            .ToList();
            return productHandlers;
    }
    public string? CreateProductsImageUrl(IPublishedContent product, string baseUrl)
    {
        var productImage= product.Value<IPublishedContent>("productImage");
        return productImage != null ? $"{baseUrl}/${productImage.Url()}":null;
    }
}
