namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisi��o HTTP que produz todas as categorias j� registradas.
/// </summary>
public static class CategoryGetAll
{
    /// <summary>
    /// O endpoint onde as categorias est�o localizadas.
    /// </summary>
    public static string Template => "/categories";

    /// <summary>
    /// Os m�todos HTTP usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethods.Get.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que busca todas as categorias j� registradas.
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
