namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisi��o HTTP que busca por uma categoria espec�fica.
/// </summary>
public static class CategoryGet
{
    /// <summary>
    /// O endpoint onde a categoria desejada est� localizada.
    /// </summary>
    public static string Template => "/categories/{id:guid}";

    /// <summary>
    /// Os m�todos HTTP usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que busca categorias.
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
