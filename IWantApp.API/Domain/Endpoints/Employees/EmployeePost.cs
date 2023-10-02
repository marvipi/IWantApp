using IWantApp.API.Domain.Services;

namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisição HTTP usada para registrar um novo empregado.
/// </summary>
public static class EmployeePost
{
    /// <summary>
    /// O endpoint onde empregados estão localizados.
    /// </summary>
    public static string Template => "/employees";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    /// <summary>
    /// Uma referência a um método que registra empregados no sistema.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action(EmployeeRequest request, HttpContext httpContext, UserCreator userCreator)
    {
        var creator = httpContext
            .User
            .Claims
            .First(claim => claim.Type == ClaimTypes.NameIdentifier)
            .Value;

        var userClaims = new List<Claim>() {
            new(ClaimTypes.Name, request.Name),
            new("EmployeeCode", request.EmployeeCode),
            new("CreatedBy", creator)
        };

        (var result, var newUserId) = await userCreator.Create(request.Email, request.Password, userClaims);

        if (!result.Succeeded) return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        return Results.Created(Template + $"/{newUserId}", newUserId);
    }
}
