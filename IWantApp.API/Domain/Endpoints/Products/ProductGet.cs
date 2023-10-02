namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uam requisição HTTP que busca por um produto específico.
/// </summary>
public class ProductGet
{
    /// <summary>
    /// O endpoint onde o produto esta localizado.
    /// </summary>
    public static string Template => "/products/{id:guid}";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca produtos.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext applicationDbContext)
    {
        IResult result;
        var product = await applicationDbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            result = Results.NotFound();
        }
        else
        {
            result = Results.Ok(new ProductResponse(product.Name,
                product.Description,
                product.Category.Name,
                product.HasStock,
                product.Price));
        }
        return result;
    }
}
