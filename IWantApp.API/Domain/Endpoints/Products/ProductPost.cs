namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uma requisição HTTP que registra um novo produto no sistema.
/// </summary>
public static class ProductPost
{
    /// <summary>
    /// O endpoint no qual os produtos estão localizados.
    /// </summary>
    public static string Template => "/products";

    /// <summary>
    /// Os métodos HTTP que usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    /// <summary>
    /// Uma referência a um método que registra novos produtos.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action(ProductRequest request, HttpContext httpContext, ApplicationDbContext context)
    {
        var user = httpContext.User
            .Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier)
            .Value;

        var category = await context.Categories.FindAsync(request.CategoryId);
        var product = new Product(request.Name, category, request.HasStock, user, request.Description);

        if (!product.IsValid) return Results.ValidationProblem(product.Notifications.ConvertToProblemDetails());

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return Results.Created(Template + $"{product.Id}", product.Id);
    }
}
