namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisi��o HTTP usada para registrar um novo empregado.
/// </summary>
public static class EmployeePost
{
    /// <summary>
    /// O endpoint onde empregados est�o localizados.
    /// </summary>
    public static string Template => "/employees";

    /// <summary>
    /// Os m�todos HTTP usados para invocar esta requisi��o.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };

    /// <summary>
    /// Uma refer�ncia a um m�todo que registra empregados no sistema.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action(EmployeeRequest request, HttpContext httpContext, UserManager<IdentityUser> userManager)
    {
        var creator = httpContext
            .User
            .Claims
            .First(claim => claim.Type == ClaimTypes.NameIdentifier)
            .Value;
        var newUser = new IdentityUser { UserName = request.Email, Email = request.Email };

        var result = await userManager.CreateAsync(newUser, request.Password);
        if (!result.Succeeded) return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var claims = new List<Claim>() {
            new(ClaimTypes.Name, request.Name),
            new("EmployeeCode", request.EmployeeCode),
            new("CreatedBy", creator)
        };

        var claimsResult = await userManager.AddClaimsAsync(newUser, claims);
        if (!claimsResult.Succeeded) return Results.ValidationProblem(claimsResult.Errors.ConvertToProblemDetails());

        return Results.Created(Template + $"/{newUser.Id}", newUser.Id);


    }
}
