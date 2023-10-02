namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisi��o HTTP que remove uma categoria do sistema.
/// </summary>
public static class CategoryDelete
{
    /// <summary>
    /// O endpoint onde a categoria que ser� removida est� localizada.
    /// </summary>
    public static string Template => "/categories/{id:guid}";

    /// <summary>
    /// Os m�todos HTTP usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethods.Delete.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que remove categorias.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var categoryToRemove = await context.Categories.FindAsync(id);
        if (categoryToRemove is null)
        {
            return Results.NotFound();
        }
        else
        {
            context.Categories.Remove(categoryToRemove);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}
