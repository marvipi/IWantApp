namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisi��o HTTP que atualiza uma categoria j� registrada.
/// </summary>
public static class CategoryPut
{
    /// <summary>
    /// O endpoint onde a categoria que ser� atualizada est� localizada.
    /// </summary>
    public static string Template => "/categories/{id:guid}";

    /// <summary>
    /// Os m�todos HTTP usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que atualiza categorias.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] Guid id, CategoryRequest request, HttpContext http, ApplicationDbContext context)
    {
        var category = await context.Categories.FindAsync(id);
        if (category is null) return Results.NotFound();

        var user = http
            .User
            .Claims
            .First(claim => claim.Type == ClaimTypes.NameIdentifier)
            .Value;
        category.Update(request.Name, request.Active, user);
        if (!category.IsValid) return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());

        await context.SaveChangesAsync();
        return Results.NoContent();
    }
}

