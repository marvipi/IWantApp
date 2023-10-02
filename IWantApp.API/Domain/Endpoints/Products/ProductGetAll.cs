namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uam requisição HTTP que busca todos os produtos registrados no sistema.
/// </summary>
public class ProductGetAll
{
    /// <summary>
    /// O endpoint onde os produtos estão localizados.
    /// </summary>
    public static string Template => "/products";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca produtos.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static IResult Action(ApplicationDbContext applicationDbContext)
    {
        var products = applicationDbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToList();
        var response = products.Select(p => new ProductResponse(p.Name, p.Description, p.Category.Name, p.HasStock, p.Active, p.Price));
        return Results.Ok(response);
    }
}
