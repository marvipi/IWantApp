namespace IWantApp.API.Domain.Endpoints.Products;

/// <summary>
/// Representa uma requisição HTTP que remove um produto do sistema.
/// </summary>
public static class ProductDelete
{
    /// <summary>
    /// O endpoint onde o produto está localizado..
    /// </summary>
    public static string Template => "/products/{id:guid}";

    /// <summary>
    /// Os métodos HTTP que usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };

    /// <summary>
    /// Uma referência a um método que remove produtos.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null) return Results.NotFound();

        context.Remove(product);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}
