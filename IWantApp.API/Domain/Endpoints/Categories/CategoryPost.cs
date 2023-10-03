using IWantApp.API.Domain.Extensions;

namespace IWantApp.API.Domain.Endpoints.Categories;

/// <summary>
/// Representa uma requisição HTTP que cria uma nova categoria.
/// </summary>
public static class CategoryPost
{
    /// <summary>
    /// O endpoint onde a nova categoria vai estar localizada.
    /// </summary>
    public static string Template => "/categories";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    /// <summary>
    /// Uma referência a um método que cria novas categorias.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action(CategoryRequest request, HttpContext http, ApplicationDbContext context)
    {
        var user = http
            .User
            .Claims
            .First(claim => claim.Type == ClaimTypes.NameIdentifier)
            .Value;
        var category = new Category(request.Name, user);

        if (!category.IsValid)
        {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }
        else
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return Results.Created(Template + $"/{category.Id}", category.Id);
        }
    }
}
