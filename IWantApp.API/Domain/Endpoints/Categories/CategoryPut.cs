namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisição HTTP que atualiza uma categoria já registrada.
/// </summary>
public static class CategoryPut
{
    /// <summary>
    /// O endpoint onde a categoria que será atualizada está localizada.
    /// </summary>
    public static string Template => "/categories/{id:guid}";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };

    /// <summary>
    /// Uma referência a um método que atualiza categorias.
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

