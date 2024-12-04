using SiteManager.ProductService;

namespace ProductValidation;

public class ProductValidator
{
    private readonly ILogger<ProductValidator> _logger;
    public ProductValidator(ILogger<ProductValidator> logger)
    {
        _logger = logger;
    }
    public bool IsProductValid(Product product, out string errormessage)
    {
        if (!IsProductTitleValid(product.Title!, out errormessage) ||
            !IsProductCategoryValid(product.Category!, out errormessage) ||
            !IsProductDescriptionValid(product.Description!, out errormessage) ||
            !IsProductPriceValid(product.Price, out errormessage) ||
            !IsProductCardDescriptionValid(product.CardDescription!, out errormessage) ||
            !IsProductImageUrlValid(product.ImageUrl!, out errormessage))
        {
            _logger.LogWarning(errormessage);
            return false;
        }
        errormessage = string.Empty;
        return true;
    }
    public static bool IsProductTitleValid(string productTitle, out string errormessage)
    {
        if (string.IsNullOrWhiteSpace(productTitle) || productTitle.Length > 100)
        {
            errormessage = "Product title must be between 1 and 100 characters.";
            return false;
        }
        errormessage = string.Empty;
        return true;
    }
    public static bool IsProductCategoryValid(string productCategory, out string errormessage)
    {
        if (string.IsNullOrWhiteSpace(productCategory) || productCategory.Length > 100)
        {
            errormessage = "Product category must be between 1 and 100 characters.";
            return false;
        }
        errormessage = string.Empty;
        return true;
    }
    public static bool IsProductDescriptionValid(string productDescription, out string errormessage)
    {
        if (string.IsNullOrWhiteSpace(productDescription) || productDescription.Length > 500)
        {
            errormessage = "Product description must be between 1 and 500 characters.";
            return false;
        }
        errormessage = string.Empty;
        return true;
    }
    public static bool IsProductPriceValid(int productPrice, out string errormessage)
    {
        if (productPrice < 0)
        {
            errormessage = "Product price must be a positive number.";
            return false;
        }
        errormessage = string.Empty;
        return true;
    }
    public static bool IsProductCardDescriptionValid(string productCardDescription, out string errormessage)
    {
        if (string.IsNullOrWhiteSpace(productCardDescription) || productCardDescription.Length > 200)
        {
            errormessage = "Product card description must be between 1 and 200 characters.";
            return false;
        }
        errormessage = string.Empty;
        return true;
    }
    public static bool IsProductImageUrlValid(string productImageUrl, out string errormessage)
    {
        if (!string.IsNullOrWhiteSpace(productImageUrl) && !Uri.TryCreate(productImageUrl, UriKind.Absolute, out _))
        {
            errormessage = "Product image URL must be a valid absolute URL.";
            return false;
        }
        errormessage = string.Empty;
        return true;
    }
}
