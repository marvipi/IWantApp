namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uam requisição HTTP que busca todos os produtos que os visitantes do site podem ver.
/// </summary>
public class ProductGetShowcase
{
    /// <summary>
    /// O endpoint onde os produtos estão localizados.
    /// </summary>
    public static string Template => "/products/showcase";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca produtos.
    /// </summary>
    public static Delegate Handler => Action;

    [AllowAnonymous]
    private static IResult Action(
        ApplicationDbContext applicationDbContext,
        [FromQuery] int page = 1,
        [FromQuery] int rows = 10,
        [FromQuery] string orderBy = "name")
    {
        if (page < 1) return Results.Problem(
            title: "Paging error",
            detail: "Page cannot be less than 1.",
            statusCode: 400);
        if (rows < 1 || rows > 10) return Results.Problem(
            title: "Paging error",
            detail: "Rows must be between 1 and 10, inclusive on both ends.",
            statusCode: 400);

        var queryBase = applicationDbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        var queryOrdered = OrdernarConsulta(queryBase, orderBy);

        if (queryOrdered is null) return Results.Problem(
            title: "Ordering error",
            detail: "Can only order by name and price.",
            statusCode: 400);

        var queryPaged = queryOrdered.Skip((page - 1) * rows).Take(rows);

        var products = queryPaged.ToList();
        var response = products.Select(p => new ProductResponse(p.Name, p.Description, p.Category.Name, p.HasStock, p.Active, p.Price));
        return Results.Ok(response);
    }

    private static IOrderedQueryable<Product>? OrdernarConsulta(IQueryable<Product> queryBase, string orderBy)
    {
        return orderBy switch
        {
            "name" => queryBase.OrderBy(p => p.Name),
            "price" => queryBase.OrderBy(p => p.Price),
            _ => null
        };
    }
}
