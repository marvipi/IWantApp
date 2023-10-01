namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisição HTTP que produz todas as categorias já registradas.
/// </summary>
public static class CategoryGetAll
{
    /// <summary>
    /// O endpoint onde as categorias estão localizadas.
    /// </summary>
    public static string Template => "/categories";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethods.Get.ToString() };

    /// <summary>
    /// Uma referência a um método que busca todas as categorias já registradas.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static IResult Action(ApplicationDbContext context)
    {
        var categories = context.Categories.ToList();
        var response = categories
            .Select(category => new CategoryResponse(category.Name, category.Active))
            .ToList();
        return Results.Ok(response);
    }
}
