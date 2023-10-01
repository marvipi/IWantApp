namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uma requisição HTTP que atualiza um produto já registrado no sistema.
/// </summary>
public static class ProductPut
{
    /// <summary>
    /// O endpoint onde o produto está localizado..
    /// </summary>
    public static string Template => "/products/{id:guid}";

    /// <summary>
    /// Os métodos HTTP que usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };

    /// <summary>
    /// Uma referência a um método que atualiza produtos.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] Guid id, ProductRequest request, HttpContext httpContext, ApplicationDbContext context)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null) return Results.NotFound();

        var user = httpContext.User
            .Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier)
            .Value;
        var category = await context.Categories.FindAsync(request.CategoryId);
        product.Update(request.Name, category, request.HasStock, user, request.Description);

        if (!product.IsValid) return Results.ValidationProblem(product.Notifications.ConvertToProblemDetails());

        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
