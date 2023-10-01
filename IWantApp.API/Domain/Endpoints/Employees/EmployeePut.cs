namespace IWantApp.API.Domain.Endpoints.Employees;

/// <summary>
/// Representa uma requisição HTTP que atualiza os dados de um empregado específico.
/// </summary>
public static class EmployeePut
{
    /// <summary>
    /// O endpoint onde o empregado está localizado.
    /// </summary>
    public static string Template => "/employees/{id}";

    /// <summary>
    /// Os métodos HTTP usados para invocar esta requisição.
    /// </summary>
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };

    /// <summary>
    /// Uma referência a um método que atualiza dados dos empregados.
    /// </summary>
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    private static async Task<IResult> Action([FromRoute] string id, EmployeeRequest request, HttpContext httpContext, UserManager<IdentityUser> userManager)
    {
        var modifier = httpContext
            .User
            .Claims
            .First(claim => claim.Type == ClaimTypes.NameIdentifier)
            .Value;

        var user = await userManager.FindByIdAsync(id);
        if (user is null) return Results.NotFound();

        user.Email = request.Email;
        user.UserName = request.Email;
        await userManager.AddClaimAsync(user, new Claim("ModifiedBy", modifier));

        var claims = await userManager.GetClaimsAsync(user);
        var employeeCode = claims.FirstOrDefault(c => c.Type == "EmployeeCode");
        await userManager.ReplaceClaimAsync(user, employeeCode, new Claim("EmployeeCode", request.EmployeeCode));

        var employeeName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        await userManager.ReplaceClaimAsync(user, employeeName, new Claim(ClaimTypes.Name, request.Name));

        await userManager.UpdateNormalizedEmailAsync(user);
        await userManager.UpdateNormalizedUserNameAsync(user);
        await userManager.UpdateAsync(user);

        return Results.NoContent();
    }
}
