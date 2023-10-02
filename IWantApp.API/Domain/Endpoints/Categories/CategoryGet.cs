namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisição HTTP que busca por uma categoria específica.
/// </summary>
public static class CategoryGet
{
    /// <summary>
    /// O endpoint onde a categoria desejada está localizada.
    /// </summary>
    public static string Template => "/categories/{id:guid}";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca categorias.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var category = await context.Categories.FindAsync(id);
        var result = category is null ? Results.NotFound() : Results.Ok(new CategoryResponse(category.Name, category.Active));
        return result;
    }
}
